(function ($) {
  $.extend($.fn, {
    validateVCode:
    function (setting) {
      if (!setting) {
        setting = {};
      }
      var ps = $.extend({
        imgId: '_vcodeImg',
        changeId: '_change'
      }, setting);
      var that = this;
      var id = that.attr('id');
      $('#' + id + ps.changeId).on('click', changeImg);
      function changeImg() {
        var vcodeImg = $('#' + id + ps.imgId);
        vcodeImg.attr("src", vcodeImg.attr("src") + "?" + Math.random());
      }
    }
  });
})(jQuery)