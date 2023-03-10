var tmpEmdId = 0;
var currentRequest = null;
var sTimeout;

//#region - Common Bind
$(document).ready(function () {
    loadEmployee(true);
});

//#endregion

//#region - AJAX calls
function SaveEmployee() {
    try {
        if (!$("#newEmpform").valid()) {
            return;
        }
        var type = 'POST';
        var url = $("#hdnBaseApiUrl").val() + "/Employee/AddEmployee";
        var employee = {
            Id: parseInt($("#hdnEmployeeId").val()),
            Name: $("#Name").val(),
            Email: $("#Email").val(),
            DOB: $("#DOB").val(),
            Department: $("#Department").val()
        };

        if (employee.Id > 0) {
            type = 'PUT';
            url = $("#hdnBaseApiUrl").val() + "/Employee/UpdateEmployee";
        }
        GetToken(function (token) {
            $.ajax({
                type: type,
                url: url,
                contentType: "application/json; charset=utf-8",
                headers: { "Authorization": "Bearer " + token },
                dataType: 'json',
                data: JSON.stringify(employee),
                success: function (response) {
                    hideLoader();
                    toastApiResponse(response);
                    loadEmployee(true);
                    hideEmployeeModal();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    toast("error", "Error occurred");
                    hideLoader();
                }
            });
        });
    }
    catch (err) {
        hideLoader();
        toast("error", err);
    }
}

function EditEmployee(Id) {
    try {
        var url = $("#hdnBaseApiUrl").val() + "/Employee/GetEmployeeById";
        GetToken(function (token) {
            $.ajax({
                type: 'GET',
                url: url + '/' + Id,
                contentType: "application/json; charset=utf-8",
                headers: { "Authorization": "Bearer " + token },
                dataType: 'json',
                success: function (response) {
                    hideLoader();
                    if (response) {
                        toastApiResponse(response);
                        $("#hdnEmployeeId").val(response.id);
                        $("#Name").val(response.name);
                        $("#Email").val(response.email);
                        $("#DOB").val(response.dob.split('T')[0]);
                        $("#Department").val(response.department);
                        $("#addNewEmployeeModal").modal('show');
                    }
                },
                error: function (err) {
                    hideLoader();
                }
            });
        });
    } catch (e) {
        hideLoader();
    }
}

function DeleteEmployee() {
    $("#confirmationModal").modal('hide');
    if (tmpEmdId != 0) {
        try {
            var url = $("#hdnBaseApiUrl").val() + "/Employee/DeleteEmployee";
            GetToken(function (token) {

                $.ajax({
                    type: 'DELETE',
                    url: url + '/' + tmpEmdId,
                    contentType: "application/json; charset=utf-8",
                    headers: { "Authorization": "Bearer " + token },
                    dataType: 'json',
                    success: function (response) {
                        hideLoader();
                        toastApiResponse(response);
                        loadEmployee(true);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        hideLoader();
                    }
                });
            });
        } catch (err) {
            hideLoader();
        }
    }
}

function SearchEmployee() {
    if (sTimeout) {
        clearTimeout(sTimeout);
    }
    sTimeout = setTimeout(function () {
        var keyword = $("#txtSearch").val();
        if (keyword.trim() != "") {
            currentRequest = $.ajax({
                url: "/Home/SearchEmployees?keyword=" + keyword,
                type: "GET",
                beforeSend: function () {
                    if (currentRequest) {
                        currentRequest.abort();
                    }
                }
            }).done(function (partialViewResult) {
                $("#empList").html(partialViewResult);
                clearTimeout(sTimeout);
                sTimeout = null;
            });
        }
        else {
            loadEmployee();
        }
    }, 10);
}

//#endregion

//#region - Private

function loadEmployee(isShowLoader) {
    if (isShowLoader) {
        showLoader();
    }
    $.ajax({
        url: "/Home/GetEmployees",
        type: "GET",
        success: function () {
            hideLoader();

        },
        error: function () {
            hideLoader();

        }
    }).done(function (partialViewResult) {
        $("#empList").html(partialViewResult);
        hideLoader();
    });
}

function clearForm() {
    $("#hdnEmployeeId").val(0);
    $("#Name").val('');
    $("#Email").val('');
    $("#DOB").val('');
    $("#Department").val('');
}

function showAddEmployeeModal() {
    clearForm();
    $("#addNewEmployeeModal").modal('show');
}

function hideEmployeeModal() {
    $("#addNewEmployeeModal").modal('hide');
}

function hideConfirmationModal() {
    tmpEmdId = 0;
    $("#confirmationModal").modal('hide');
}

function showDeleteConfirmationModal(Id) {
    tmpEmdId = Id;
    $("#confirmationModal").modal('show');
}

function onEditEmployee(Id) {
    EditEmployee(Id);
}

function toastApiResponse(resp) {
    if (resp && resp.responseCode) {
        var type = resp.responseCode == "200" ? "success" : "error";
        toast(type, resp.message);
    }
}

function toast(type, message) {
    $.toast({
        autoDismiss: true,
        type: type,
        message: message
    });
}

function GetToken(successCallback) {
    showLoader();
    try {
        $.ajax({
            type: 'GET',
            url: "Home/GetAccessToken",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response && successCallback && typeof successCallback == "function") {
                    successCallback(response)
                }
            },
            error: function (err) {
                hideLoader();
            }
        });
    } catch (e) {
        hideLoader();
    }
}

function showLoader() {
    $("#ajaxSpinner").show();
}

function hideLoader() {
    $("#ajaxSpinner").hide();
}
//#endregion