(function ($) {
  $.extend($.fn, {
    groupForeignKeyForCheck:
    function (setting) {
      if (!setting) {
        setting = {};
      }
      var ps = $.extend({
        checkid: '_check',
        textid: '_text',
        selectid: '_select',
        windowid: '_window',
        saveid: '_save',
        checkall: '_checkall',
        reset: '_reset'
      }, setting);
      var that = this;
      var id = that.attr('id');
      function binding() {
        setValue();
        $('#' + id + ps.selectid).on('click', function () {
          $("#" + id + ps.windowid).data("kendoWindow").center().open();
        })
        $('#' + id + ps.saveid).on('click', saveData);
        $('.' + id + ps.checkall).on('click', checkAll);
        $('.' + id + ps.reset).on('click', reset);
      }

      function setValue() {
        var selectText = $.map($('[name="' + id + ps.checkid + '"]:checked'), function (item) { return $(item).data('text') }).join(',');
        $('#' + id + ps.textid).val(selectText);
        $('#' + id + ps.textid).attr('title', selectText);
        var selectValus = $.map($('[name="' + id + ps.checkid + '"]:checked'), function (item) { return $(item).val() }).join(',');
        that.val(selectValus);
      }
      function saveData() {
        setValue();
        that.parents('form:first').validate().element('#' + id);
        $('#' + id + ps.windowid).data("kendoWindow").close();
      }
      function reset(e) {
        var target = $(e.currentTarget);
        var container = target.parents('.k-checklist-group');
        container.find('[name="' + id + ps.checkid + '"]').prop('checked', false);
      }
      function checkAll(e) {
        var target = $(e.currentTarget);
        var container = target.parents('.k-checklist-group');
        container.find('[name="' + id + ps.checkid + '"]').prop('checked', true);
      }
      binding();
    }
  })
})(jQuery)