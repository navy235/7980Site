(function () {
    var JHelper = {
        showMessage: function (res) {
            var $message;
            if (res.Success) {
                $message = $('.jsmessage');
            } else {
                $message = $('.jserrmessage');
            }
            $message.find(".action-text").text(res.Message);
            $message.addClass("out").show();
        }
    }
    window.Maitonn = window.Maitonn || {};
    window.Maitonn.JHelper = JHelper;
})()
