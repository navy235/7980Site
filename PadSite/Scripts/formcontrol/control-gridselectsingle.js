(function ($) {
  $.extend($.fn, {
    gridSelectSingle:
    function (setting) {
      if (!setting) {
        setting = {};
      }
      var ps = $.extend({
        windowId: '_window',
        gridId: '_grid',
        saveId: '_save',
        searchId: '_search',
        keywordsId: '_keywords',
        selectId: '_select',
        infoId: '_info',
        serverUrl: '',
        getUrl: '',
        checkName: 'ID_check',
        displayName: '',
        value: 0
      }, setting);
      var that = this;
      var id = that.attr('id');

      $('#' + id + ps.selectId).on('click', showgrid);

      $('#' + id + ps.searchId).on('click', search);

      $('#' + id + ps.saveId).on('click', save);

      if (ps.value != 0) {
        $.get(ps.getUrl, { id: ps.value }, function (res) {
          $('#' + id + ps.infoId).html(res);
        })
      }

      function showgrid() {
        initGrid();
        var gridWindow = $('#' + id + ps.windowId).data('kendoWindow');
        gridWindow.center().open();
      }

      function search() {
        var value = $('#' + id + ps.keywordsId).val();
        var grid = $('#' + id + ps.gridId).data('kendoGrid');
        if (value != '') {
          grid.dataSource.filter({ field: "Name", operator: "contains", value: value });
        } else {
          grid.dataSource.filter({});
        }
      }

      function save() {
        if ($('[name="' + ps.checkName + '"]:checked').size() == 0) {
          alert("请选择" + ps.displayName);
        } else {
          var checkId = $('[name="' + ps.checkName + '"]:checked').val();
          var grid = $('#' + id + ps.gridId).data('kendoGrid');
          var item = grid.dataSource.get(checkId);
          $('#' + id + ps.infoId).html(item.Name);
          that.val(checkId);
          that.parents('form:first').validate().element('#' + id);
          $('#' + id + ps.windowId).data('kendoWindow').close();
        }
      }

      function initGrid() {
        jQuery('#' + id + ps.gridId).kendoGrid({
          "columns": [{
            "title": "选择",
            "attributes": {
              "style": "text-align:center"
            },
            "width": "50px",
            "template": "\u003cinput type=\u0027radio\u0027 name=\u0027ID_check\u0027 id=\u0027#=MemberID#_check\u0027  class=\u0027chkbx\u0027 value=\u0027#=MemberID#\u0027 /\u003e",
            "field": "MemberID",
            "sortable": false,
            "filterable": false,
            "encoded": true
          },
          {
            "title": "公司名称",
            "field": "Name",
            "encoded": true
          }],
          "pageable": {
            "buttonCount": 10,
            "messages": {
              "display": "显示条目 {0} - {1} 共 {2}",
              "empty": "没有可显示的记录。",
              "page": "页",
              "of": "共 {0}",
              "refresh": "刷新"
            }
          },
          "sortable": true,
          "dataSource": {
            "transport": {
              "read": {
                "url": ps.serverUrl
              }
            },
            "pageSize": 10,
            "page": 1,
            "total": 0,
            "serverPaging": true,
            "serverSorting": true,
            "serverFiltering": true,
            "serverGrouping": true,
            "serverAggregates": true,
            "type": "aspnetmvc-ajax",
            "filter": [],
            "sort": [{
              "field": "MemberID",
              "dir": "desc"
            }],
            "schema": {
              "data": "Data",
              "total": "Total",
              "errors": "Errors",
              "model": {
                "id": "MemberID",
                "fields": {
                  "MemberID": {
                    "type": "number"
                  },
                  "Name": {
                    "type": "string"
                  }
                }
              }
            }
          }
        });
      }
    }
  });

})(jQuery)