﻿(function () {
  var dateFormat = function () {
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
      timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
      timezoneClip = /[^-+\dA-Z]/g,
      pad = function (val, len) {
        val = String(val);
        len = len || 2;
        while (val.length < len) val = "0" + val;
        return val;
      };

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc) {
      var dF = dateFormat;

      // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
      if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
        mask = date;
        date = undefined;
      }

      // Passing date through Date applies Date.parse, if necessary
      date = date ? new Date(date) : new Date;
      if (isNaN(date)) throw SyntaxError("invalid date");

      mask = String(dF.masks[mask] || mask || dF.masks["default"]);

      // Allow setting the utc argument via the mask
      if (mask.slice(0, 4) == "UTC:") {
        mask = mask.slice(4);
        utc = true;
      }

      var _ = utc ? "getUTC" : "get",
        d = date[_ + "Date"](),
        D = date[_ + "Day"](),
        m = date[_ + "Month"](),
        y = date[_ + "FullYear"](),
        H = date[_ + "Hours"](),
        M = date[_ + "Minutes"](),
        s = date[_ + "Seconds"](),
        L = date[_ + "Milliseconds"](),
        o = utc ? 0 : date.getTimezoneOffset(),
        flags = {
          d: d,
          dd: pad(d),
          ddd: dF.i18n.dayNames[D],
          dddd: dF.i18n.dayNames[D + 7],
          m: m + 1,
          mm: pad(m + 1),
          mmm: dF.i18n.monthNames[m],
          mmmm: dF.i18n.monthNames[m + 12],
          yy: String(y).slice(2),
          yyyy: y,
          h: H % 12 || 12,
          hh: pad(H % 12 || 12),
          H: H,
          HH: pad(H),
          M: M,
          MM: pad(M),
          s: s,
          ss: pad(s),
          l: pad(L, 3),
          L: pad(L > 99 ? Math.round(L / 10) : L),
          t: H < 12 ? "a" : "p",
          tt: H < 12 ? "am" : "pm",
          T: H < 12 ? "A" : "P",
          TT: H < 12 ? "AM" : "PM",
          Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
          o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
          S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
        };

      return mask.replace(token, function ($0) {
        return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
      });
    };
  }();

  // Some common format strings
  dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
  };

  // Internationalization strings
  dateFormat.i18n = {
    dayNames: [
      "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
      "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    monthNames: [
      "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
      "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ]
  };

  // For convenience...
  Date.prototype.format = function (mask, utc) {
    return dateFormat(this, mask, utc);
  };


  Date.prototype.addDays = function (num) {
    var value = this.valueOf();
    value += 86400000 * num;
    return new Date(value);
  }

  Date.prototype.addSeconds = function (num) {
    var value = this.valueOf();
    value += 1000 * num;
    return new Date(value);
  }

  Date.prototype.addMinutes = function (num) {
    var value = this.valueOf();
    value += 60000 * num;
    return new Date(value);
  }

  Date.prototype.addHours = function (num) {
    var value = this.valueOf();
    value += 3600000 * num;
    return new Date(value);
  }

  Date.prototype.addMonths = function (num) {
    var value = new Date(this.valueOf());

    var mo = this.getMonth();
    var yr = this.getYear();

    mo = (mo + num) % 12;
    if (0 > mo) {
      yr += (this.getMonth() + num - mo - 12) / 12;
      mo += 12;
    }
    else
      yr += ((this.getMonth() + num - mo) / 12);

    value.setMonth(mo);
    value.setYear(yr);
    return value;
  }

  var Cookies = {};

  Cookies.Set = function (name, value) {
    var argv = arguments;
    var argc = arguments.length;
    var expires = (argc > 2) ? argv[2] : null;
    var path = (argc > 3) ? argv[3] : '/';
    var domain = (argc > 4) ? argv[4] : null;
    var secure = (argc > 5) ? argv[5] : false;
    document.cookie = name + "=" + escape(value) +
       ((expires == null) ? "" : ("; expires=" + expires.toGMTString())) +
       ((path == null) ? "" : ("; path=" + path)) +
       ((domain == null) ? "" : ("; domain=" + domain)) +
       ((secure == true) ? "; secure" : "");
  };

  Cookies.Get = function (name) {
    var arg = name + "=";
    var alen = arg.length;
    var clen = document.cookie.length;
    var i = 0;
    var j = 0;
    while (i < clen) {
      j = i + alen;
      if (document.cookie.substring(i, j) == arg)
        return Cookies.GetCookieVal(j);
      i = document.cookie.indexOf(" ", i) + 1;
      if (i == 0)
        break;
    }
    return null;
  };

  Cookies.Clear = function (name) {
    if (Cookies.Get(name)) {
      var expdate = new Date();
      expdate.setTime(expdate.getTime() - (86400 * 1000 * 1));
      Cookies.Set(name, "", expdate);
    }
  };

  Cookies.GetCookieVal = function (offset) {
    var endstr = document.cookie.indexOf(";", offset);
    if (endstr == -1) {
      endstr = document.cookie.length;
    }
    return unescape(document.cookie.substring(offset, endstr));
  };

  window.Maitonn = window.Maitonn || {};
  window.Maitonn.Cookies = Cookies;

  var JHelper = {
    checkLogin: function () {
      var d = $.Deferred();
      var UserCookie = Maitonn.Cookies.Get("mtUser");
      var isLogin = !(UserCookie == null || UserCookie.indexOf("UID") < 0);
      d.resolve(isLogin);
      return d.promise();
    },
    hasInitPopLogin: false,
    initPopLogin: function () {
      $('#loginContent').load(function () {
        $('#loginLoading').hide();
        $(this).show();
      });
    },
    showPopLogin: function () {
      if (!this.hasInitPopLogin) {
        this.initPopLogin();
        this.hasInitPopLogin = true;
      }
      $('#loginContent').hide();
      $('#loginLoading').show();
      var loginUrl = $('#popLoginUrl').val();
      var loginWindow = $('#Win-PopLogin').data('kendoWindow');
      $('#loginContent').attr('src', loginUrl + '?' + Math.random());
      loginWindow.center().open();
    },
    showMessage: function (res) {
      var $message;
      if (res.Success) {
        $message = $('.jsmessage');
      } else {
        $message = $('.jserrmessage');
      }
      $message.find(".action-text").text(res.Message);
      $message.show();
      $message.fadeOut(3000)
    },
    checkFavorite: function (id) {
      var d = $.Deferred();
      $.get(ajaxUrl.check_favorite, { id: id }, function (res) {
        d.resolve(res);
      });
      return d.promise();
    },
    addFavorite: function (id) {
      var d = $.Deferred();
      $.post(ajaxUrl.add_favorite, { id: id }, function (res) {
        d.resolve(res);
      });
      return d.promise();
    },
    getMediaPeriodCode: function (id) {
      var d = $.Deferred();
      $.get(ajaxUrl.get_mediaPeriodCode, { id: id }, function (res) {
        d.resolve(res);
      });
      return d.promise();
    },
    getSchemeForm: function () {
      var d = $.Deferred();
      $.get(ajaxUrl.get_schemeform, {}, function (res) {
        d.resolve(res);
      });
      return d.promise();
    },
    getScheme: function () {
      var d = $.Deferred();
      $.get(ajaxUrl.get_scheme, {}, function (res) {
        d.resolve(res);
      });
      return d.promise();
    },
    addScheme: function (data) {
      var d = $.Deferred();
      $.post(ajaxUrl.add_scheme, data, function (res) {
        d.resolve(res);
      });
      return d.promise();
    },
    editSchemeMedia: function (data) {
      var d = $.Deferred();
      $.post(ajaxUrl.edit_schemeMedia, data, function (res) {
        d.resolve(res);
      });
      return d.promise();
    },

    addCompareItem: function (item) {
      var comparelist = $('#media-compare-list');
      var div = $('<div/>').attr({
        'class': 'media-compare-item',
        'data-id': item.id
      }).html(item.title);
      div.append('<i class="icon-maitonn icon-remove media-compare-remove"></i>')
      comparelist.show().find('.media-compare-button').before(div);
    },
    addCompare: function (e) {
      var target = $(e.currentTarget);
      var currentValue = this.getCompare();
      if (currentValue.length >= 5) {
        alert("最多选择5项进行对比");
        return false;
      }
      var cValue = target.data("value")
      for (var i = 0; i < currentValue.length; i++) {
        if (currentValue[i].id == cValue) {
          alert("您已经选择了该项");
          return false;
        }
      }
      var item = {
        id: cValue,
        title: decodeURIComponent(target.data("title"))
      };
      currentValue.push(item);
      this.addCompareItem(item);
      this.setCompare(currentValue);
    },
    getCompare: function () {
      var currentValue = Cookies.Get("compare");
      var compares = [];
      if (!currentValue)
        return compares;
      var currentCompare = currentValue.split(',');
      for (var i = 0; i < currentCompare.length; i++) {
        var compareItem = currentCompare[i].split('|');
        compares.push({ id: compareItem[0], title: compareItem[1] });
      }
      return compares;
    },
    setCompare: function (values) {
      var valueArr = [];
      for (var i = 0; i < values.length; i++) {
        valueArr.push(values[i].id + "|" + values[i].title);
      }
      Cookies.Set("compare", valueArr.join(","));
    },
    removeCompare: function (e) {
      var target = $(e.currentTarget).parents('.media-compare-item');
      var id = target.data('id');
      var compares = this.getCompare();
      for (var i = 0; i < compares.length; i++) {
        if (compares[i].id == id) {
          compares.splice(i, 1);
        }
      }
      this.setCompare(compares);
      this.removeCompareItem(id);
    },
    removeCompareItem: function (id) {
      var comparelist = $('#media-compare-list');
      comparelist.find('[data-id="' + id + '"]').remove();
      if (comparelist.find('.media-compare-item').size() == 0) {
        comparelist.hide();
      }
    },
    clearCompare: function () {
      var comparelist = $('#media-compare-list');
      comparelist.find('.media-compare-item').remove();
      comparelist.hide();
      Cookies.Set("compare", "");
    },
    showCompare: function () {
      var compares = this.getCompare();
      if (compares.length > 0) {
        for (var i = 0; i < compares.length; i++) {
          var item = compares[i];
          this.addCompareItem(item);
        }
      }
    },
    jumptoCompare: function () {
      var compares = this.getCompare();
      var compareIds = $.map(compares, function (item) { return item.id }).join(',');
      window.location.href = ajaxUrl.compare_media + '/index/' + compareIds;
    },
    downloadFangan: function () {
      var periodNumber = $('#periodNumber').val();
      var periodCate = $('#periodCate').val();
      var mediaIds = $('#mediaIds').val();
      var dq = $('#dq').val();
      window.location.href = ajaxUrl.download_fangan + '?periodNumber=' + periodNumber + '&periodCate=' + periodCate + '&mediaIds=' + mediaIds + '&dq=' + dq;
    },
    printFangan: function () {
      var periodNumber = $('#periodNumber').val();
      var periodCate = $('#periodCate').val();
      var mediaIds = $('#mediaIds').val();
      var dq = $('#dq').val();
      window.location.href = ajaxUrl.print_fangan + '?periodNumber=' + periodNumber + '&periodCate=' + periodCate + '&mediaIds=' + mediaIds + '&dq=' + dq;
    },
    listFavorite: function (e) {
      var self = this,
          target = $(e.currentTarget),
          mediaId = target.data('value');
      self.checkLogin().done(function (isLogin) {
        if (isLogin) {
          var kwindow = $('#Win-favorite').data('kendoWindow');
          self.checkFavorite(mediaId).done(function (result) {
            if (result.Success) {
              self.addFavorite(mediaId).done(function (res) {
                if (res.Success) {
                  kwindow.center().open();
                } else {
                  self.showMessage(res);
                }
              })
            } else {
              self.showMessage(result);
            }
          })
        } else {
          self.showPopLogin();
        }
      });
    },
    listScheme: function (e) {
      var self = this,
          target = $(e.currentTarget),
          price = target.data('price'),
          src = target.data('src'),
          title = target.data('title'),
          mediaId = target.data('value');
      self.checkLogin().done(function (isLogin) {
        if (isLogin) {
          price = price == 99999 ? 0 : price / 365;
          var kwindow = $('#Win-scheme').data('kendoWindow');
          if (!$('.goods-addscheme')[0]) {
            self.getSchemeForm().done(function (html) {
              $('#goods-addscheme-info-img').attr({
                src: src,
                title: title
              });
              $('#goods-addscheme-info-text').html(title)
              $('#goods-addscheme-info').data('id', mediaId).after(html);
              self.getMediaPeriodCode(mediaId).done(function (res) {
                $("#period").kendoDropDownList({
                  dataTextField: "Text",
                  dataValueField: "Value",
                  dataSource: res,
                  index: 0,
                  change: changePeriodCode
                });
                var today = new Date();
                $('[name="startTime"]').kendoDatePicker();
                var startTime = $('[name="startTime"]').data('kendoDatePicker');
                startTime.min(today);
                startTime.value(today);
                $('[name="number"]').kendoNumericTextBox({
                  "format": "n0",
                  "decimals": 0,
                  "change": changePeriodCode,
                  "spin": changePeriodCode
                })
                var number = $('[name="number"]').data('kendoNumericTextBox');
                number.min(1);

                function changePeriodCode() {
                  if (price == 0) {
                    $('.goods-price').html("面议")
                  } else {
                    var periodValue = $("#period").val();
                    var numberValue = number.value();
                    var $price = $('.goods-price').find('b');
                    $price.text((periodValue * numberValue * price).toFixed(2));
                    $('#price').val((periodValue * numberValue * price).toFixed(2));
                  }
                }

                $('[name="addtype"]').on('click', $.proxy(self.addtypeClick, self))

                $('#btn-comfirm-scheme').on('click', $.proxy(self.comfirmSchemeClick, self))

                $('#Name').on('keyup', $.proxy(self.checkForm, self));

                $('#btn-cancle-scheme').on('click', function () {
                  $('#Win-scheme').data('kendoWindow').close();
                })
                changePeriodCode();
              })

            })
          }
          setTimeout(function () {
            kwindow.center().open();
          }, 1000)
        } else {
          self.showPopLogin();
        }
      });


    },
    addtypeClick: function (e) {
      var self = this,
          target = $(e.currentTarget),
          value = target.val();
      if (value == 1) {
        $('[name="Name"]').parents('.form-field').show();
        $('[name="Description"]').parents('.form-field').show();
        $('[name="schemeId"]').parents('.form-field').hide();
      } else {
        if ($(this).attr('disabled') == 'disabled') {
        } else {
          $('[name="Name"]').parents('.form-field').hide();
          $('[name="Description"]').parents('.form-field').hide();
          $('[name="schemeId"]').parents('.form-field').show();
        }
      }
    },
    comfirmSchemeClick: function (e) {
      var self = this,
          target = $(e.currentTarget);
      if (self.checkForm()) {
        var mediaId = $('#goods-addscheme-info').data('id');
        var addType = $('[name="addtype"]:checked').val();
        var kwindowSuccess = $('#Win-scheme-success').data('kendoWindow');
        var kwindow = $('#Win-scheme').data('kendoWindow');
        var data = {};
        if (addType == 1) {
          data.Name = $('#Name').val();
          data.Description = $('#Description').val();
          data.schemeId = 0;

        } else {
          data.Name = '';
          data.Description = '';
          data.schemeId = $('#schemeId').val();
        }

        data.periodCode = parseInt($('#period').val());

        data.periodCount = parseInt($('#number').val());

        data.startTime = $('#startTime').val();

        var endTime = new Date($('#startTime').val()).addDays(($('#number').val() * $('#period').val()));

        data.endTime = endTime.format('yyyy/mm/dd');

        data.id = mediaId;

        data.price = $('#price').val();

        self.addScheme(data).done(function (res) {
          if (res.Success) {
            kwindow.close();
            $('.goods-addscheme').remove();
            kwindowSuccess.center().open();
          } else {
            kwindow.close();
            $('.goods-addscheme').remove();
            self.showMessage(res);
          }
        })
      } else {
        return false;
      }
    },
    checkForm: function () {
      var addType = $('[name="addtype"]:checked').val();
      var $form_tips = $('.form-tips-error').find('span');
      var validateErrors = [];
      if (addType == 1) {
        var schemeName = $('#Name').val();
        if (schemeName == '') {
          $form_tips.text('请填写方案名称');
          validateErrors.push('请填写方案名称');
        } else if (schemeName.length > 30) {
          $form_tips.text('方案名称最多30个字');
          validateErrors.push('方案名称最多30个字');
        }
      }
      if (validateErrors.length == 0) {
        $('.form-tips-error').hide();
      } else {
        $('.form-tips-error').show();
        return false;
      }
      return validateErrors.length == 0;
    }
  }
  window.Maitonn = window.Maitonn || {};
  window.Maitonn.JHelper = JHelper;
})()
