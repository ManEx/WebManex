var filter = "";
var rootUrl = $("#rootUrl").val().toLowerCase();
var changedLicenseType = false;


var $account = {
    changePw: function (homeUrl) {
        $(".ui-widget").css("display", "none");
        var newPw1 = $("#newPw1").val();
        var newPw2 = $("#newPw2").val()
        if (newPw1 != newPw2) {
            $("#pwDontMatch").css("display", "block");
        }
        else {

            $.ajax({
                url: rootUrl + "/Account/ChangePassword",
                data:
                {
                    oldPassword: $("#currPw").val(),
                    newPassword: $("#newPw1").val()
                },
                success: function (data) {
                    if (data.success != null && data.success == "true") {
                        $("#changeSuccess").css("display", "block");
                        setTimeout(function () { document.location.href = homeUrl; }, 3000);
                    }
                    else {
                        $("#error").css("display", "block");
                        if (data.error != null) {
                            $("#errMsg").html(data.error);
                        }
                    }
                },
                failure: function () {
                    $("#error").css("display", "block");
                },
                type: "POST"
            });
        }
    }
};

