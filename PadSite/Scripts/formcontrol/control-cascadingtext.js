(function ($) {
  $.extend($.fn, {
    cascadingtext:
    function (setting) {
      if (!setting) {
        setting = {};
      }
      var ps = $.extend({
        cascadingIds: ''
      }, setting);
      var that = this;
      var id = that.attr('id');
      var inited = false;
      var cascadingIds = ps.cascadingIds.split(',');
      $('#' + id).on('keydown', function () {
        inited = true;
      })
      for (var i = 0; i < cascadingIds.length; i++) {
        var cId = cascadingIds[i];
        $('#' + cId).parents('.form-field').on('change', 'select', setText);
      }

      function setText() {
        if (!inited) {
          var text = '';
          for (var i = 0; i < cascadingIds.length; i++) {
            var cId = cascadingIds[i];
            text += '-' + getElementText($('#' + cId));
          }
          if (text.substr(0, 1) == '-') {
            text = text.substr(1);
          }
          $('#' + id).val(text);
        }
      }

      function getElementText(element) {
        var texts = $.map(element.parent().find('select'), function (item) {
          if ($(item).find('option:selected').text() == '请选择') {
            return '';
          } else {
            return $(item).find('option:selected').text();
          }
        })
        return texts.join('-');
      }
    }
  });

})(jQuery)