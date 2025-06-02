mergeInto(LibraryManager.library,
{
  IsHandheldDeviceJS: function()
  {
    const _userAgent = navigator.userAgent || navigator.vendor || window.opera;
    const _mobileRegex = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini|Windows Phone|Mobile|mobile|CriOS/i;
    
    if (_mobileRegex.test(_userAgent))
    {
        return true;
    }
    
    if (navigator.userAgentData && navigator.userAgentData.mobile) 
    {
        return true;
    }
    
    const _hasTouch = 'ontouchstart' in window || navigator.maxTouchPoints > 0 || navigator.msMaxTouchPoints > 0;
    const _isCoarsePointer = window.matchMedia && window.matchMedia('(pointer: coarse)').matches;
    const _hasOrientation = 'orientation' in window;
    
    return _hasTouch || _isCoarsePointer || _hasOrientation;
  }
});
