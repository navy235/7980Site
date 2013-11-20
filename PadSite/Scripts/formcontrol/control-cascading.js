(function ($) {
  $.extend($.fn, {
    cascading:
    function (setting) {
      if (!setting) {
        setting = {};
      }
      var ps = $.extend({
        read_url: '',
        get_url: '',
        values: '',
        paramId: 'pid',
        optionLabel: '请选择',
        value: '',
        maxlevel: 0,
        result: []
      }, setting);

      var that = this;

      var id = that.attr('id');

      var proxy = $.proxy;

      var form = $('#' + id).parents('form:first');

      var container = $('#' + id).parents('.form-cascading');

      var init = false;

      var data = {};

      container.on('change', 'select', onChange)

      function binding() {
        if (ps.values === '') {
          getResponse(0);
        } else {
          ps.result = ps.values.split(',');
          for (var i = 0; i < ps.result.length; i++) {
            if (i === 0) {
              data[ps.paramId] = 0;
            } else {
              data[ps.paramId] = ps.result[i - 1];
            }
            getResponse(i, 0, ps.result[i], true);
          }
          setValue(ps.values);
        }
      }

      function getResponse(level, value, defaultvalue, isInit) {
        $.get(ps.read_url, data, function (res) {
          if (res.length > 0) {
            createSelect(level + 1, res, defaultvalue, isInit);
            if (ps.maxlevel != 0 && level == ps.maxlevel) {
              setValue();
              validate();
            }
          } else {
            clearLevel(level + 1);
            setValue();
            validate();
          }
        })
      }

      function createSelect(level, listdata, defaultvalue, isInit) {
        var selector;
        if (checkLevel(level)) {
          selector = $('#' + id + level);
          selector.empty();
        } else {
          selector = $('<select/>').attr({
            'id': id + level,
            'name': id + level,
            'data-level': level
          })
        }
        addOptionLabel(selector);
        for (var i = 0; i < listdata.length; i++) {
          var option = $('<option/>').attr({
            value: listdata[i].Value
          }).text(listdata[i].Text)
          if (listdata[i].Selected) {
            option.attr('checked', true);
          }
          selector.append(option);
        }

        container.append(selector);

        if (defaultvalue) {
          selector.val(defaultvalue);
        }
        sortSelect();

        if (!isInit) {
          clearLevel(level + 1);
        }
      }

      function sortSelect() {
        var selectors = container.find('select');
        $.each(selectors, function (index, item) {
          var level = $(item).data('level');
          if (level == 1) {
            container.prepend(item);
          } else {
            $(item).before($('#' + id + (level - 1)));
          }
        })
      }
      function checkLevel(level) {
        if ($('#' + id + level)[0]) {
          return true;
        }
        return false;
      }

      function disableLevel(level) {
        if (checkLevel(level)) {
          var selector = $('#' + id + level);
          selector.empty().attr('disabled', 'disabled');
          addOptionLabel(selector);
          disableLevel(level + 1);
        }
      }

      function addOptionLabel(selector) {
        var optionLabel = $('<option/>').attr({
          value: '',
        }).text(ps.optionLabel)
        selector.append(optionLabel);
      }

      function enableLevel(level) {
        if (checkLevel(level)) {
          var selector = $('#' + id + level);
          selector.empty().removeAttr('disabled');
          //selector.show();
          addOptionLabel(selector);
          //enableLevel(level + 1);
        }
      }

      function clearLevel(level) {
        if (checkLevel(level)) {
          var selector = $('#' + id + level);
          selector.remove();
          clearLevel(level + 1);
        }
      }


      function setValue(value) {
        if (value) {
          $('#' + id).val(value);
        } else {
          var values = $.map(container.find('select'), function (item) {
            return $(item).val();
          })
          var valueStr = values.join(',');
          if (valueStr.substr(valueStr.length - 1, 1) == ',') {
            valueStr = valueStr.substr(0, valueStr.length - 1);
          }
          $('#' + id).val(valueStr);
        }
      }
      function clearValue() {
        $('#' + id).val('');
      }

      function onChange(e) {
        var target = $(e.currentTarget),
            value = target.val(),
            hasNext = false,
            level = target.data('level');
        if (value === '') {
          disableLevel(level + 1);
          clearValue();
          validate();
        } else {
          enableLevel(level + 1);
          data[ps.paramId] = value;
          clearValue();
          getResponse(level, value)
        }
      }

      function validate() {
        form.validate().element('#' + id);
      }

      binding();
    }
  });
})(jQuery)