var filter = "";
var rootUrl = $("#rootUrl").val().toLowerCase();
var changedLicenseType = false;


// ******************** Functions ****************** //
var $admin = {
    superUserToggle: function () {
        var licType = $("#licenseTypeDD");
        if ($("#companyAdminChB").attr('checked') ||
            $("#acctAdminChB").attr('checked') ||
            $("#prodAdminChB").attr('checked')) {
            licType.attr("disabled", true);
            licType.val('full');
        }
        else {
            licType.attr("disabled", false);
        }
    },
    getUserListView: function (lastUserId) {
        $.ajax({
            url: rootUrl + "Admin/UserAdmin/List",
            success: function (data) {
                var container = $("#leftColContainer")[0];
                container.innerHTML = data;
                container.style.display = "block";
                if (lastUserId) {
                    var activeUser = $("#" + lastUserId)[0];
                    activeUser.className = "leftColRow active";
                }
            },
            cache: false,
            type: "GET"
        });
    },
    getEditUserView: function (userId) {
        this.clearActive();
        var activeUser = $("#" + userId)[0];
        activeUser.className = "leftColRow active";
        $("#rolesModal").remove();
        var container = $("#editUserContainer");
        container.html("<div style=\"margin:20px\">" + $("#loadingTxt").val() + "</div>");
        $.ajax({
            url: rootUrl + "Admin/UserAdmin/Edit/" + userId,
            success: function (data) {

                container.html(data);
                container.css("display", "block");
                $("#registerNewContainer").html("");
                $admin.initRoleModal();
                $global.initDialogs();
                $("#licenseTypeDD").val($("#licenseTypeTxt").val());
                changedLicenseType = false;
                $admin.superUserToggle();
            },
            cache: false,
            type: "GET"
        });
    },
    initRoleModal: function () {
        $("#rolesModal").dialog({
            autoOpen: false,
            cache: false,
            modal: true,
            height: 140
        });
        $("#viewRolesBtn").click(function () {
            $("#rolesModal").dialog("open");
        });
    },
    getRegisterNewView: function () {
        this.clearActive();
        $.ajax({
            url: rootUrl + "Admin/UserAdmin/AddUser",
            success: function (data) {
                var container = $("#registerNewContainer")[0];
                container.innerHTML = data;
                container.style.display = "block";
                $("#editUserContainer").html("");
                $global.initDialogs();
            },
            cache: false,
            type: "GET"
        });
    },
    clearActive: function () {
        var oldActiveUser = $(".active");
        for (var i = 0; i < oldActiveUser.length; i++) {
            oldActiveUser[i].className = "leftColRow";
        }
    },
    clearUserSeats: function (userId) {
        $.ajax({
            url: rootUrl + "Admin/UserAdmin/ClearUserSeats",
            data:
            {
                userId: userId
            },
            success: function () {
                $("#clearSuccess").css("display", "block");
                $admin.getUserListView();
            },
            failure: function () {
                $("#clearError").css("display", "block");
            },
            type: "POST"
        });
    },
    
    validateEdit: function () {
        var noEmptyFields = true;
        noEmptyFields = $global.validateNotEmpty('usernameLbl', 'usernameTxt');
        noEmptyFields = $global.validateNotEmpty('initialsLbl', 'initialsTxt');
        noEmptyFields = $global.validateNotEmpty('fNameLbl', 'fNameTxt');
        noEmptyFields = $global.validateNotEmpty('lNameLbl', 'lNameTxt');
        noEmptyFields = $global.validateNotEmpty('emailLbl', 'emailTxt');
        noEmptyFields = $global.validateNotEmpty('licenseTypeLbl', 'licenseTypeDD');
        if (noEmptyFields) { this.editUser(); }
        else { $("#regMissingFields").css("display", "block"); }
    },
    deleteUser: function () {
        if (confirm($("#confirmDeleteTxt").val())) {
            $.ajax({
                url: rootUrl + "Admin/UserAdmin/EditUser",
                data:
            {
                del: "true",
                userId: $("#userIdTxt").val(),
                userName: $("#userNameTxt").val()
            },
                success: function () {
                    $("#deleteConfirm").css("display", "block");
                    $("#currentUserInfo").css("display", "none");
                    $admin.getUserListView();
                },
                failure: function () {
                    $("#editError").css("display", "block");
                },
                type: "POST"
            });
        }
    },
    editUser: function () {
        $("#editError").css("display", "none");
        $("#editConfirm").css("display", "none");
        $.ajax({
            url: rootUrl + "Admin/UserAdmin/EditUser",
            data:
            {
                edit: "true",
                initials: $("#initialsTxt").val(),
                firstName: $("#fNameTxt").val(),
                lastName: $("#lNameTxt").val(),
                emailAddr: $("#emailTxt").val(),
                isCompanyAdmin: $("#companyAdminChB:checked").val(),
                isAcctAdmin: $("#acctAdminChB:checked").val(),
                isProdAdmin: $("#prodAdminChB:checked").val(),
                isExempt: $("#exemptChB:checked").val(),
                isExternal: $("#externalChB:checked").val(),
                cantChangePw: $("#cantChangePwChB:checked").val(),
                changePw: $("#forceChangePwChB:checked").val(),
                pwExpireInterval: $("#pwExpIntervalTxt").val(),
                userId: $("#userIdTxt").val(),
                userSuspended: $("#userApprovedChB:checked").val(),
                suppliers: $global.getListItems("editSupplierChB"),
                customers: $global.getListItems("editCustomerChB"),
                groups: $global.getListItems("editGroupChB"),
                changedLicType: changedLicenseType,
                licenseType: $("#licenseTypeDD").val()
            },
            success: function (data) {
                if (data.success != null && data.success == "true") {
                    $("#editConfirm").css("display", "block");
                    $admin.getUserListView($("#userIdTxt").val());
                }
                else {
                    $("#editError").css("display", "block");
                    $("#errMsg")[0].innerHTML = "<br /><br />" + data.error;
                }
            },
            failure: function () {
                $("#editError").css("display", "block");
            },
            type: "POST"
        });
    },
    validateAdd: function () {
        $global.clearValidation('registerNewContainer');
        var noEmptyFields = true;
        noEmptyFields = $global.validateNotEmpty('regUsernameLbl', 'regUsernameTxt');
        noEmptyFields = $global.validateNotEmpty('regInitialsLbl', 'regInitialsTxt');
        noEmptyFields = $global.validateNotEmpty('regFNameLbl', 'regFNameTxt');
        noEmptyFields = $global.validateNotEmpty('regLNameLbl', 'regLNameTxt');
        noEmptyFields = $global.validateNotEmpty('regEmailLbl', 'regEmailTxt');
        noEmptyFields = $global.validateNotEmpty('licenseTypeLbl', 'licenseTypeDD');

        if (noEmptyFields) { this.addUser(); }
        else { $("#missingFields").css("display", "block"); }
    },
    addUser: function () {
        $("#saveError").css("display", "none");
        $("#saveConfirm").css("display", "none");
        $.ajax({
            url: rootUrl + "Admin/UserAdmin/AddUser",
            data:
            {
                username: $("#regUsernameTxt").val(),
                initials: $("#regInitialsTxt").val(),
                firstName: $("#regFNameTxt").val(),
                lastName: $("#regLNameTxt").val(),
                emailAddr: $("#regEmailTxt").val(),
                isCompanyAdmin: $("#regCompanyAdminChB:checked").val(),
                isAcctAdmin: $("#acctAdminChB:checked").val(),
                isProdAdmin: $("#prodAdminChB:checked").val(),
                isExempt: $("#exemptChB:checked").val(),
                isExternal: $("#externalChB:checked").val(),
                cantChangePw: $("#cantChangePwChB:checked").val(),
                userActive: "on",
                suppliers: $global.getListItems("supplierChB"),
                customers: $global.getListItems("customerChB"),
                groups: $global.getListItems("groupChB"),
                licenseType: $("#licenseTypeDD").val()
            },
            success: function () {
                $("#saveConfirm").css("display", "block");
                $admin.getUserListView();
            },
            failure: function () {
                $("#saveError").css("display", "block");
            },
            type: "POST"
        });
    },    
    filterGroup: function () {
        var option = $('#filterGroupDd').val();
        var groupRows = $('.groupRow');
        if (option == "All") { groupRows.css("display", "table-row"); }
        else {
            for (var i = 0; i < groupRows.length; i++) {
                var roles = $("#" + groupRows[i].id + " .roleChB");
                for (var r = 0; r < roles.length; r++) {
                    if (option == "Assigned") {
                        if (roles[r].checked) {
                            groupRows[i].style.display = "table-row";
                            break;
                        }
                        else { groupRows[i].style.display = "none"; }
                    }
                    else { // option = Unassigned
                        if (roles[r].checked) {
                            groupRows[i].style.display = "none";
                            break;
                        }
                        else { groupRows[i].style.display = "table-row"; }
                    }
                }
            }
        }
    },    
    getAddGroupView: function () {
        $.ajax({
            url: rootUrl + "Admin/Groups/Add",
            success: function (data) {
                var container = $("#addGroupContainer")[0];
                container.innerHTML = data;
                container.style.display = "block";
                $("#groupDetailsContainer").html("");
                $global.initDialogs();
            },
            cache: false,
            type: "GET"
        });
    },
    addNewGroup: function () {
        $(".ui-widget").css("display", "none");
        if ($("#groupNameTxt").val() != "") {
            $.ajax({
                url: rootUrl + "Admin/Groups/AddGroup",
                data:
                {
                    groupName: $("#groupNameTxt").val(),
                    groupDesc: $("#groupDescTxt").val(),
                    groupId: $("#groupId").val()
                },
                success: function (data) {
                    if (data.success === "true") {
                        var id = $("#groupId").val();
                        var name = $("#groupNameTxt").val();
                        $admin.getGroupListView(id, name, true);
                    }
                    else {
                        $("#addError").css("display", "block");
                        $("#errMsg").html(data.error);
                    }
                },
                type: "POST"
            });
        }
        else {
            $("#groupNameTxt").css("background-color", "pink");
            $("#groupNameReq").css("display", "block");
        }
    },
    deleteGroup: function (groupId) {
        $(".ui-widget").css("display", "none");
        $.ajax({
            url: rootUrl + "Admin/Groups/DeleteGroup",
            data:
            {
                groupId: groupId
            },
            success: function (data) {
                if (data.success === "true") {
                    $("#delConfirm").css("display", "block");
                    $(".group").css("display", "none");
                    $admin.getGroupListView(null, null, false);
                }
                else {
                    $("#delError").css("display", "block");
                    $("#errMsg").html(data.error);
                }
            },
            type: "POST"
        });

    },
    getGroupListView: function (groupId, groupName, adding) {
        $.ajax({
            url: rootUrl + "Admin/Groups/List",
            success: function (data) {
                var container = $("#leftColContainer")[0];
                container.innerHTML = data;
                container.style.display = "block";
                if (groupId) {
                    $admin.getGroupView(groupId, groupName, adding);
                }
            },
            cache: false,
            type: "GET"
        });
    },
    getGroupView: function (groupId, groupName, adding) {
        this.clearActive();
        var activeGroup = $("#" + groupId)[0];
        activeGroup.className = "leftColRow active";
        var url = rootUrl + "Admin/Groups/Edit/" + groupId + "?groupName=" + groupName + "&add=false"; ;
        if (adding) {
            url = rootUrl + "Admin/Groups/Edit/" + groupId + "?groupName=" + groupName + "&add=true";
        }
        var container = $("#groupDetailsContainer");
        container.html("<div style=\"margin:20px\">" + $("#loadingTxt").val() + "</div>");
        $.ajax({
            url: url,
            success: function (data) {
                container.html(data);
                $("#addGroupContainer").html("");
                $global.initDialogs();
            },
            cache: false,
            type: "GET"
        });
    },
    editGroup: function () {
        $("#editError").css("display", "none");
        $("#editConfirm").css("display", "none");
        $.ajax({
            url: rootUrl + "Admin/Groups/EditGroup",
            data:
            {
                edit: "true",
                groupId: $("#groupIdTxt").val(),
                groupName: $("#groupNameTxt").val(),
                description: $("#groupNameTxt").val(),
                roles: $global.getListItems("roleChB")
            },
            success: function (data) {
                if (data.success != null && data.success == "true") {
                    $("#editConfirm").css("display", "block");
                    $admin.getGroupListView(groupId, $("#groupNameTxt").val(), false);
                }
                else {
                    $("#editError").css("display", "block");
                    $("#errMsg")[0].innerHTML = "<br /><br />" + data.error;
                }
            },
            failure: function () {
                $("#editError").css("display", "block");
            },
            type: "POST"
        });
    },
    delGroupConf: function () {
        if (confirm($("#delConfTxt").val())) {
            $admin.deleteGroup($("#groupIdTxt").val());
        }
    },
    toggleRoles: function () {
        if ($("#allRolesChB").prop("checked")) {
            $(".roleChB").prop("checked", true);
        }
        else {
            $(".roleChB").prop("checked", false);
        }
    },
    toggleClassRoles: function (chBId, className) {
        if ($("#" + chBId).prop("checked")) {
            $("." + className).prop("checked", true);
        }
        else {
            $("." + className).prop("checked", false);
        }
    }
};

