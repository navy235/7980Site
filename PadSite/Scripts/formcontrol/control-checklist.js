﻿(function ($) {
  $.extend($.fn, {
    checkList:
    function (setting) {
      if (!setting) {
        setting = {};
      }
      var ps = $.extend({
        checkid: '_check'
      }, setting);
      var that = this;
      var id = that.attr('id');
      var inited = false;
      function binding() {
        setValue();
        $('[name="' + id + ps.checkid + '"]').on('click', setValue);
        inited = true;
      }
      function setValue() {
        var selectValus = $.map($('[name="' + id + ps.checkid + '"]:checked'), function (item) { return $(item).val() }).join(',');
        that.val(selectValus);
        if (inited) {
          that.parents('form:first').validate().element('#' + id);
        }
      }
      binding();
    }
  });

})(jQuery)