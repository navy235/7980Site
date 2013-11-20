(function ($) {
  $.extend($.fn, {
    smsVCode:
    function (setting) {
      if (!setting) {
        setting = {};
      }
      var ps = $.extend({
        getId: '_get',
        mobileId: 'mobile'
      }, setting);
      var that = this;
      var id = that.attr('id');
      $('#' + id + ps.getId).on('click', getVCode);
      var interval = 60;
      var changeTextInterval = null;
      var button = $('#' + id + ps.getId);
      var mobile = $('#' + ps.mobileId);
      var reg = /(13[0-9]|15[0-9]|18[0-9])\d{8}/;
      var form = that.parents('form:first');
      function getVCode() {
        var url = button.data('url');
        if (!button.prop('disabled')) {
          if (reg.test(mobile.val())) {
            //$.get(url, {}, function (message) {
            button.prop('disabled', true);
            //that.val(1254)
            changeTextInterval = setInterval(changeButtonStatus, 1000);
            //})
          } else {
            form.validate().element('#' + ps.mobileId);
          }
        } else {
          return false;
        }
      }
      function changeButtonStatus() {
        interval--;
        if (interval > 0) {
          button.text("免费获取验证码 " + "( " + interval + " )");
        }
        else {
          interval = 60;
          button.prop('disabled', false);
          button.text('免费获取验证码');
          clearInterval(changeTextInterval);
        }
      }
    }
  });
})(jQuery)