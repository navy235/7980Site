(function ($) {
  $.extend($.fn, {
    areaSelect:
    function (setting) {
      if (!setting) {
        setting = {};
      }
      var ps = $.extend({
        heightId: '_height',
        widthId: '_width',
        faceId: '_face',
        changeRegularId: '_isRegular',
        regularClass: 'form-areas-regular',
        irregularClass: 'form-areas-irregular',
        decimalsClass: 'form-areas-decimals',
        intClass: 'form-areas-int',
        resultId: '_result',
        addId: '_add',
        deleteClass: '_delete',
        elementId: '_area',
        value: ''
      }, setting);
      var that = this;
      var id = that.attr('id');
      var element = $('#' + id + ps.elementId);
      var addButton = $('#' + id + ps.addId);
      element.on('click', '#' + id + ps.addId, addAreaSinge)
      element.on('click', '.' + id + ps.deleteClass, deleteArea)
      element.on('click', '[name="' + id + ps.changeRegularId + '"]', changeRegular)

      function binding() {
        var totalAreas = 1;
        var isRegular = element.find('[name="' + id + ps.changeRegularId + '"]:checked').val() == "true";
        initFormItem($('.' + ps.regularClass));
        initFormItem($('.' + ps.irregularClass));
        if (ps.value != '') {
          if (isRegular) {
            var values = ps.value.split('|');
            var inputs = $('.' + ps.regularClass).find('input[data-role="numerictextbox"]');
            for (var i = 1; i < values.length; i++) {
              $(inputs[i - 1]).data('kendoNumericTextBox').value(values[i]);
              totalAreas *= values[i];
            }
          } else {
            totalAreas = 0;
            var values = ps.value.split('|');
            if (values.length > 3) {
              var totalface = (values.length - 1) / 2;
              for (var i = 0; i < (totalface - 1) ; i++) {
                addAreaSinge();
              }
            }
            var inputs = $('.' + ps.irregularClass).find('input[data-role="numerictextbox"]');
            for (var i = 1; i < values.length; i += 2) {
              $(inputs[i - 1]).data('kendoNumericTextBox').value(values[i]);
              $(inputs[i]).data('kendoNumericTextBox').value(values[i + 1]);
              totalAreas += (values[i] * values[i + 1])
            }
          }
        } else {
          totalAreas = 0;
        }
        $('#' + id + ps.resultId).html(totalAreas);
      }

      function changeRegular() {
        var isRegular = element.find('[name="' + id + ps.changeRegularId + '"]:checked').val() == "true";
        var formClass = isRegular ? ps.regularClass : ps.irregularClass;
        if (isRegular) {
          $('.' + ps.regularClass).show()
          $('.' + ps.irregularClass).hide();
        } else {
          $('.' + ps.regularClass).hide()
          $('.' + ps.irregularClass).show();
        }
        setValue();
      }

      function addAreaSinge() {

        var formitem = $('<div class="form-areas-single">'
            + '<span class="form-areas-item">宽： '
            + '<input type="text"  class="form-areas-decimals" name="' + id + '_windth" />m * '
            + '</span>'
            + '<span class="form-areas-item">高： '
            + '<input type="text"  class="form-areas-decimals" name="' + id + '_height"  />m * '
            + '</span>'
            + '<span class="form-areas-item">'
               + ' <button type="button" name="' + id + '_delete"  class="k-button ' + id + '_delete" >'
                   + ' <span class="k-icon k-delete"></span>删除该面'
                + '</button>'
           + ' </span>'
           + '</div>');

        addButton.before(formitem);

        initFormItem(formitem);
      }

      function deleteArea(e) {
        var target = $(e.currentTarget);
        target.parents('.form-areas-single').remove();
        setValue();
      }

      function initFormItem(formitem) {
        var decimalsInputs = formitem.find('.form-areas-decimals');
        $.each(decimalsInputs, function (index, item) {
          $(item).kendoNumericTextBox(
          {
            "spin": areaChange,
            "change": areaChange
          }).attr({ "min": 0 });
        })
        var intInputs = formitem.find('.form-areas-int');
        $.each(intInputs, function (index, item) {
          $(item).kendoNumericTextBox(
              {
                "spin": areaChange,
                "change": areaChange, "format": "n0", "decimals": 0
              }).attr({ "min": 1 });
        })
      }
      function areaChange() {
        setValue();
      }

      function setValue() {
        var params = [];
        var totalAreas = 1;
        var isRegular = element.find('[name="' + id + ps.changeRegularId + '"]:checked').val() == "true";
        params.push(isRegular);
        var formClass = isRegular ? ps.regularClass : ps.irregularClass;
        $.each(element.find('.' + formClass).find('input[data-role="numerictextbox"]'), function (index, item) {
          params.push($(item).val());
        });
        if (isRegular) {
          for (var i = 1; i < params.length; i++) {
            totalAreas *= params[i];
          }
        } else {
          totalAreas = 0;
          for (var i = 1; i < params.length; i += 2) {
            totalAreas += (params[i] * params[i + 1])
          }
        }
        totalAreas = totalAreas.toFixed(2);
        $('#' + id + ps.resultId).html(totalAreas);
        $('#' + id).val(params.join('|'));
        that.parents('form:first').validate().element('#' + id);
      }
      binding();
    }
  });

})(jQuery)