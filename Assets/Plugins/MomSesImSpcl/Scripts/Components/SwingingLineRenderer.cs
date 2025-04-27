using MomSesImSpcl.Extensions;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Swings a <see cref="LineRenderer"/> on the <see cref="Vector2.x"/> axis.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public sealed class SwingingLineRenderer : MonoBehaviour
    {
        #region Inspector Fields
        [Header("References")]
        [Tooltip("The object that is attached to the end of the line renderer.")]
        [SerializeField] private Transform attachedObject;
        
        [Header("Settings")]
        [Tooltip("The speed with which the line swings.")]
        [SerializeField] private float swingSpeed = 1.5f;
        [Tooltip("The maximum angle the line will swing to.")]
        [SerializeField] private float swingAngle = 30f;
        [Tooltip("The amount by which the line will be curved while swinging.")]
        [SerializeField] private float curvatureAmount = 0.25f;
        [Tooltip("The length of the line renderer.")]
        [SerializeField] private float lineRendererLength = 10f;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.OnValueChanged(nameof(this.SetLineRendererSegments))]
#endif
        [Tooltip("The number of positions that will be created in the line renderer.")]
        [SerializeField] private int lineRendererSegments = 10;
        #endregion
        
        #region Fields
        /// <summary>
        /// Gradually increases by <see cref="Time.deltaTime"/>.
        /// </summary>
        private float swingTime;
        /// <summary>
        /// The <see cref="LineRenderer"/> that displays the swinging line.
        /// </summary>
        private LineRenderer lineRenderer;
        /// <summary>
        /// Holds the <see cref="Transform"/> of the <see cref="attachedObject"/>.
        /// </summary>
        private TransformAccessArray attachedObjectTransform;
        /// <summary>
        /// Holds the <see cref="Transform.position"/> values for the <see cref="lineRendererSegments"/>.
        /// </summary>
        private NativeArray<Vector3> lineRendererPositions;
        /// <summary>
        /// <see cref="JobHandle"/> for the <see cref="AttachedObjectJob"/>.
        /// </summary>
        private JobHandle attachedObjectJobHandle;
        /// <summary>
        /// <see cref="JobHandle"/> for the <see cref="LineRendererJob"/>.
        /// </summary>
        private JobHandle lineRendererJobHandle;
        #endregion
        
        #region Methods
        private void Awake()
        {
            this.lineRenderer = base.GetComponent<LineRenderer>();
            this.lineRenderer.useWorldSpace = true;
            this.SetLineRendererSegments(this.lineRendererSegments);
            
            this.attachedObjectTransform = new TransformAccessArray(1);
            this.attachedObjectTransform.Add(this.attachedObject);
        }
        
        private void OnDestroy()
        {
            this.attachedObjectJobHandle.Complete();
            this.lineRendererJobHandle.Complete();
            
            if (this.attachedObjectTransform.isCreated)
                this.attachedObjectTransform.Dispose();
            
            if (this.lineRendererPositions.IsCreated)
                this.lineRendererPositions.Dispose();
        }
        
        private void Update()
        {
            this.swingTime += Time.deltaTime;

            var _position = base.transform.Position3();
            var _angle = math.sin(this.swingTime * this.swingSpeed) * this.swingAngle;
            var _attachedObjectJob = new AttachedObjectJob
            (
                _Angle:              _angle,
                _LineRendererLength: this.lineRendererLength,
                _AnchorPosition:     _position
            );
            var _lineRendererJob = new LineRendererJob
            (
                _LineRendererPositions:  this.lineRendererPositions,
                _AnchorPosition:         _position,
                _AttachedObjectPosition: this.attachedObject.Position3(),
                _CurvatureAmount:        this.curvatureAmount,
                _Angle:                  _angle,
                _SwingAngle:             this.swingAngle,
                _LineRendererSegments:   this.lineRendererSegments
            );
            
            this.attachedObjectJobHandle = _attachedObjectJob.Schedule(this.attachedObjectTransform);
            this.lineRendererJobHandle = _lineRendererJob.Schedule(this.attachedObjectJobHandle);
            
            JobHandle.ScheduleBatchedJobs();
        }
        
        private void LateUpdate()
        {
            this.attachedObjectJobHandle.Complete();
            this.lineRendererJobHandle.Complete();
            this.lineRenderer.SetPositions(this.lineRendererPositions);
        }
        
         /// <summary>
         /// Sets the position count in the <see cref="lineRenderer"/>.
         /// </summary>
         /// <param name="_LineRendererSegments">The number of positions the <see cref="lineRenderer"/> should have.</param>
        private void SetLineRendererSegments(int _LineRendererSegments)
        {
            if (this.lineRendererPositions.IsCreated)
            {
                this.lineRendererPositions.Dispose();
            }
            
            this.lineRendererPositions = new NativeArray<Vector3>(_LineRendererSegments + 1, Allocator.Persistent);
            this.lineRenderer.positionCount = this.lineRendererPositions.Length;
        }
        #endregion
        
        #region Jobs
        /// <summary>
        /// Job to set the <see cref="Transform.position"/> and <see cref="Transform.rotation"/> of the <see cref="attachedObject"/>:
        /// </summary>
        [BurstCompile]
        private readonly struct AttachedObjectJob : IJobParallelForTransform
        {
            #region Fields
            /// <summary>
            /// The angle with which to calculate the position.
            /// </summary>
            [ReadOnly] private readonly float angle;
            /// <summary>
            /// <see cref="SwingingLineRenderer.lineRendererLength"/>.
            /// </summary>
            [ReadOnly] private readonly float lineRendererLength;
            /// <summary>
            /// The root <see cref="Transform.position"/> from where the swinging is calculated.
            /// </summary>
            [ReadOnly] private readonly float3 anchorPosition;
            #endregion
            
            #region Constructors
            /// <summary>
            /// <see cref="AttachedObjectJob"/>.
            /// </summary>
            /// <param name="_Angle"><see cref="angle"/>.</param>
            /// <param name="_LineRendererLength"><see cref="lineRendererLength"/>.</param>
            /// <param name="_AnchorPosition"><see cref="anchorPosition"/>.</param>
            internal AttachedObjectJob(float _Angle, float _LineRendererLength, float3 _AnchorPosition)
            {
                this.angle = _Angle;
                this.lineRendererLength = _LineRendererLength;
                this.anchorPosition = _AnchorPosition;
            }
            #endregion
            
            #region Methods
            public void Execute(int _, TransformAccess _Transform)
            {
                var _radians = this.angle * math.PI / 180f;
                var _offset = new float3(math.sin(_radians) * this.lineRendererLength, -math.cos(_radians) * this.lineRendererLength, 0f);
                var _objectPosition = this.anchorPosition + _offset;
                var _objectRotation = quaternion.AxisAngle(new float3(0, 0, 1), _radians);
                
                _Transform.position = _objectPosition;
                _Transform.rotation = _objectRotation;
            }
            #endregion
        }
        
        /// <summary>
        /// Job to set the positions of the <see cref="LineRenderer"/> segments.
        /// </summary>
        [BurstCompile]
        private struct LineRendererJob : IJob
        {
            #region Fields
            /// <summary>
            /// Will contain the positions to pass to the <see cref="LineRenderer"/>.
            /// </summary>
            [WriteOnly] private NativeArray<Vector3> lineRendererPositions;
            /// <summary>
            /// The root <see cref="Transform.position"/> from where the swinging is calculated.
            /// </summary>
            [ReadOnly] private readonly float3 anchorPosition;
            /// <summary>
            /// The <see cref="Transform.position"/> of the <see cref="attachedObject"/>.
            /// </summary>
            [ReadOnly] private readonly float3 attachedObjectPosition;
            /// <summary>
            /// <see cref="SwingingLineRenderer.curvatureAmount"/>.
            /// </summary>
            [ReadOnly] private readonly float curvatureAmount;
            /// <summary>
            /// The angle with which to calculate the position.
            /// </summary>
            [ReadOnly] private readonly float angle;
            /// <summary>
            /// <see cref="SwingingLineRenderer.swingAngle"/>.
            /// </summary>
            [ReadOnly] private readonly float swingAngle;
            /// <summary>
            /// <see cref="SwingingLineRenderer.lineRendererSegments"/>.
            /// </summary>
            [ReadOnly] private readonly int lineRendererSegments;
            #endregion
            
            #region Constructors
            /// <summary>
            /// <see cref="LineRendererJob"/>.
            /// </summary>
            /// <param name="_LineRendererPositions"><see cref="lineRendererPositions"/>.</param>
            /// <param name="_AnchorPosition"><see cref="anchorPosition"/>.</param>
            /// <param name="_AttachedObjectPosition"><see cref="attachedObjectPosition"/>.</param>
            /// <param name="_CurvatureAmount"><see cref="curvatureAmount"/>.</param>
            /// <param name="_Angle"><see cref="angle"/>.</param>
            /// <param name="_SwingAngle"><see cref="swingAngle"/>.</param>
            /// <param name="_LineRendererSegments"><see cref="lineRendererSegments"/>.</param>
            internal LineRendererJob(NativeArray<Vector3> _LineRendererPositions, float3 _AnchorPosition, float3 _AttachedObjectPosition, float _CurvatureAmount, float _Angle, float _SwingAngle, int _LineRendererSegments)
            {
                this.lineRendererPositions = _LineRendererPositions;
                this.anchorPosition = _AnchorPosition;
                this.attachedObjectPosition = _AttachedObjectPosition;
                this.curvatureAmount = _CurvatureAmount;
                this.angle = _Angle;
                this.swingAngle = _SwingAngle;
                this.lineRendererSegments = _LineRendererSegments;
            }
            #endregion
            
            #region Methods
            public void Execute()
            {
                this.lineRendererPositions[0] = this.anchorPosition;
                this.lineRendererPositions[this.lineRendererSegments] = this.attachedObjectPosition;
                
                var _directionVector = this.attachedObjectPosition - this.anchorPosition;
                var _perpendicularDirection = math.normalize(new float3(-_directionVector.y, _directionVector.x, 0));
                var _curveStrength = this.curvatureAmount * math.sign(this.angle) * math.abs(this.angle) / this.swingAngle;
                var _controlOffset = _perpendicularDirection * _curveStrength;
                
                // ReSharper disable once InconsistentNaming
                for (var i = 1; i < this.lineRendererSegments; i++)
                {
                    var _t = (float)i / this.lineRendererSegments;
                    var _straightPosition = math.lerp(this.anchorPosition, this.attachedObjectPosition, _t);
                    var _curveFactor = 4 * _t * (1 - _t);
                    var _curveOffset = _controlOffset * _curveFactor;
                    
                    this.lineRendererPositions[i] = _straightPosition + _curveOffset;
                }
            }
            #endregion
        }
        #endregion
    }
}