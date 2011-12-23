var rootUrl = $("#rootUrl").val();
var $global = {
    // **********************  Global Validation **************************** //
    validateNotEmpty: function (labelId, textId) {
        var textField = $("#" + textId);
        var nonWhiteSpaceVal = textField.val().replace(/\s/g, '');
        if (textField && nonWhiteSpaceVal == "") {
            textField.addClass("invalidTxt");
            var labelField = $("#" + labelId);
            labelField.addClass("invalidLabel");
            return false;
        }
        return true;
    },
    clearValidation: function (containerId) {
        var textFields = $("#" + containerId + " .invalidTxt");
        for (var i = 0; i < textFields.length; i++) {
            textFields[i].className = textFields[i].className.replace("invalidTxt", "");
        }
        var labelFields = $("#" + containerId + " .invalidLabel");
        for (var i = 0; i < labelFields.length; i++) {
            labelFields[i].className = labelFields[i].className.replace("invalidLabel", "");
        }
    },

    //*********************  Misc *************************** //
    changeUpperRadius: function (id) {
        $("#" + id).css("border-top-left-radius", "2px");
        $("#" + id).css("border-top-right-radius", "2px");
        $("#" + id).css("-moz-border-radius-topright", "2px");
        $("#" + id).css("-moz-border-radius-topleft", "2px");
        $("#" + id).css("-webkit-border-top-right-radius", "2px");
        $("#" + id).css("-webkit-border-top-left-radius", "2px");
    },
    setDropDownVal: function (ddId, value) {
        $("#" + ddId).val(value);
    },

    addRoleToGroup: function (role, id) {
        $("#success_" + id).css("display", "none");
        $("#error_" + id).css("display", "none");
        if ($("#dropDown_" + id).val() != "") {
            $.ajax({
                url: rootUrl + "Admin/Groups/AddRolesToGroups",
                data:
            {
                role: role,
                groups: $global.getListItems(id + "_roleGroup")
            },
                success: function (data) {
                    if (data.success != null && data.success == "true") {
                        $("#success_" + id).css("display", "block");
                    }
                    else {
                        $("#error_" + id).css("display", "block");
                        $("#errorMsg_" + id).html(data.error);
                    }

                },
                failure: function () {
                    $("#error_" + id).css("display", "block");
                },
                type: "POST"
            });
        }
        else {
            $("#addRoleToGroupError").css("display", "block");
        }
    },
    onlyNumbers: function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        status = "";
        return true;
    },
    // *********************** Small Table/chb list functionality ******************//
    getListItems: function (chBxClass) {
        var items = $("." + chBxClass);
        var list = "";
        var itemsCount = 0;
        for (var i = 0; i < items.length; i++) {
            if (items[i].checked) {
                if (itemsCount > 0) {
                    list += "," + items[i].id;
                }
                else { list += items[i].id; }
                itemsCount++;
            }
        }
        return list;
    },
    filterSmallTable: function (className, ddId, rowId) {
        var option = $('#' + ddId).val();
        var chBItems = $('.' + className);
        for (var i = 0; i < chBItems.length; i++) {
            var rowItem = document.getElementById(rowId + chBItems[i].id);
            if (option == "All") {
                rowItem.style.display = "block";
                rowItem.style.borderTop = "none";
            }
            else if (option == "Assigned") {
                var x = chBItems[i].checked;
                if (chBItems[i].checked) {
                    rowItem.style.display = "block";
                    rowItem.style.borderTop = "none";
                }
                else { rowItem.style.display = "none"; }
            }
            else if (option == "Unassigned") {
                if (chBItems[i].checked == false) {
                    rowItem.style.display = "block";
                    rowItem.style.borderTop = "none";
                }
                else {
                    rowItem.style.display = "none";
                }
            }
        }
    },
    // *********************** Global Dialogs ******************//

    initDialogs: function () {
        $(".reqRoleModal").dialog({ autoOpen: false, modal: true });
    },
    openDialog: function (id, title) {
        $("#" + id).dialog("option", "title", title);
        $("#" + id).dialog("open");
    },


    //*******************  Nav Search  ************************//
    navSearch: function (userId) {
        $("#searchNav #searchResults").css("display", "none");
        $.ajax({
            url: rootUrl + "NavSearch",
            data:
            {
                searchText: $("#topPanel #searchText").val(),
                userId: userId,
                selectedType: $("#searchDd2").val()
            },
            success: function (data) {
                $("#searchNav #searchResults").html(data);
                $("#searchNav #searchResults").css("display", "block");
            },
            failure: function () {
                $("#editError").css("display", "block");
            },
            type: "POST"
        });
    },
    showNavSrDetails: function (id) {
        $(".detailItem").css("display", "none");
        $(".catRow").removeClass("active");
        $("#" + id).show(300);
        $("#row_" + id).addClass("active");

    }
};
