mergeInto(LibraryManager.library,
{
  IsHandheldDevice: function()
  {
    const _userAgent = navigator.userAgent || navigator.vendor || window.opera;
    const _mobileRegex = /Android|BlackBerry|CriOS|IEMobile|iPad|iPhone|iPod|Mobile|mobile|Opera Mini|webOS|Windows Phone/i;
    
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
