$(document).ready(function () {
    adminUserController.init();
});

var userConfig = {
    pageSize: 10,
    pageIndex: 1
};
var kpiLevelConfig = {
    pageSize: 10,
    pageIndex: 1
};
var adminUserController = {
    init: function () {
        adminUserController.loadData();
        adminUserController.GetListAllPermissions(0);

        adminUserController.registerEvent();

    },
    registerEvent: function () {
        $('#search').off('keypress').on('keypress', function (e) {
            if (e.which === 13) {
                adminUserController.loadData(true);
            }
        });
        $('#box select').off('change').on('change', function (e) {
            var code = $(this).parent().children('.code').text();
            adminUserController.loadDataKPILevel(true, code);
        });
        //Save KPI
        $('#btnAdd').off('click').on('click', function () {
            adminUserController.addData();
            adminUserController.loadData();

        });
        //Delete
        $('#btnDelete').off('click').on('click', function () {

        });
        //Update
        $('.btnUpdate').off('click').on('click', function () {
            //load detail
            adminUserController.loadDetail(Number($(this).data('id')));
        });
        //save
        $('#btnSaveUpdateModal').off('click').on('click', function () {
            adminUserController.updateData();
            adminUserController.loadData();
        });
        //delete
        $('.btnDelete').off('click').on('click', function () {
            adminUserController.deleteData($(this).data('id'));
        });
    },
    addData: function () {
        var res = adminUserController.validate();
        if (res === false) {
            return false;
        }
        var mObj = {
            Code: $('#addUser .Code').val(),
            Username: $('#addUser .Username').val(),
            Password: $('#addUser .Password').val(),
            LevelID: $('#addUser .LevelID').val(),
            Alias: $('#addUser .Alias').val(),
            Permission: $('#addUser .permission input[type=radio]:checked').val(),
            FullName: $('#addUser .FullName').val(),
            Email: $('#addUser .Email').val(),
            Skype: $('#addUser .Skype').val()
        };
        $.ajax({
            url: "/AdminUser/Add",
            data: JSON.stringify(mObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result === 1) {
                    success('Add successfully!')
                    $("#modal-group").modal('hide');
                    adminUserController.loadData(true);
                    adminUserController.resetForm();
                }
                else {
                    error('This code has already existed!');
                }

            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    },
    deleteData: function (id) {
        var ID = id;
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: "/AdminUser/Delete/" + ID,
                    type: "POST",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        success('Delete successfully!');
                        adminUserController.loadData();
                        $("#modal-group").modal('hide');
                    },
                    error: function (errormessage) {
                        console.log(errormessage.responseText);
                    }
                });
                success('Reacord has been deleted.');
            }
        });

    },
    resetForm: function () {
        $('.ID').val("");
        $('.Username').val("");
        $('.Password').val("");
        $('.Code').val("");
        $('.LevelID').val("");
        $('.Role').val("");
        $('.FullName').val("");
        $('.Alias').val("");
        $('.Alias').css('border-color', 'lightgrey');
        $('.Username').css('border-color', 'lightgrey');
        $('.Password').css('border-color', 'lightgrey');
        $('.Code').css('border-color', 'lightgrey');
        $('.LevelID').css('border-color', 'lightgrey');
        $('.Role').css('border-color', 'lightgrey');
        $('.FullName').css('border-color', 'lightgrey');
    },
    updateData: function () {
        var mObj = {
            ID: $('#updateUser .ID').val(),
            Username: $('#updateUser .Username').val(),
            Alias: $('#updateUser .Alias').val(),
            Permission: $('#updateUser .permission input[type=radio]:checked').val(),
            FullName: $('#updateUser .FullName').val(),
            Code: $('#updateUser .Code').val(),
            Email: $('#updateUser .Email').val(),
            Skype: $('#updateUser .Skype').val()
        };

        $.ajax({
            url: "/AdminUser/Update",
            data: JSON.stringify(mObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    Toast.fire({
                        type: 'success',
                        title: 'Update successfully!.'
                    });
                   
                    adminUserController.resetForm();
                    $("#modal-group2").modal('hide');
                    adminUserController.loadData(true);

                }

            },
            error: function (error) {
                console.log(error);
            }
        });
    },
    updateKPILevel: function (KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID) {
        var mObj = {
            KPICode: KPICode,
            Code: CodeKPILevel,
            UserCheck: UserCheck,
            Checked: Checked,
            WeeklyChecked: WeeklyChecked,
            MonthlyChecked: MonthlyChecked,
            QuaterlyChecked: QuaterlyChecked,
            YearlyChecked: YearlyChecked,
            Weekly: Weekly,
            Monthly: Monthly,
            Quaterly: Quaterly,
            Yearly: Yearly,
            TimeCheck: TimeCheck,
            TableID: TableID
        };

        $.ajax({
            url: "/AdminUser/UpdateKPILevel",
            data: JSON.stringify(mObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result) {
                    Toast.fire({
                        type: 'success',
                        title: 'Update successfully!.'
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    },
    loadDetail: function (id) {
        var value = id;
        $.ajax({
            url: "/AdminUser/GetbyID/",
            data: { ID: JSON.stringify(value) },
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                $('#updateUser .ID').val(result.ID);
                $('#updateUser .Username').val(result.Username);
                $('#updateUser .Code').val(result.Code);
                $('#updateUser .Alias').val(result.Alias);
                $('#updateUser .LevelID').val(result.LevelID);
                $('#updateUser .permission input').val(result.Permission);
                $('#updateUser .FullName').val(result.FullName);
                $('#updateUser .Email').val(result.Email);
                $('#modal-group2').modal('show');
                adminUserController.GetListAllPermissions(result.ID);
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    GetListAllPermissions: function (userid) {
        $.ajax({
            url: '/AdminUser/GetListAllPermissions/',
            data: { userid: JSON.stringify(userid) },
            type: "GET",
            dataType: "json",
            success: function (data) {
                console.table(data);
                //$("#modal-group2 select, #modal-group select").empty();
                // $("#modal-group2 select,#modal-group select").append('<option value="">.: Choose Permission :.</option>');
                var html = '';

                //console.log(data);
                $.each(data, function (key, item) {
                    //$(".Role").append("<option value=\"" + item.ID + "\">" + item.PermissionName + "</option>");
                    html += '<div class="pretty p-icon p-round">';

                    if (item.State) {
                        html += '<input type="radio" value="' + item.ID + '" checked name="radio" />';
                    } else {
                        html += '<input type="radio" value="' + item.ID + '" name="radio" />';
                    }
                    html += '<div class="state p-danger">';
                    html += '<i class="icon fa fa-check"></i>';
                    html += ' <label>' + item.PermissionName + '</label>';
                    html += '</div>';
                    html += '</div>';
                });
                $("#updateUser .permission, #addUser .permission").html(html);
            }
        });
    },
    lockUser: function (id) {
        var ID = id;
        $.ajax({
            url: "/AdminUser/LockUser/",
            data: { ID: JSON.stringify(id) },
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                success('Successfully');
                adminUserController.loadData();
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });

    },
    validate: function () {
        var isValid = true;
        if ($('.Username').val().trim() === "") {
            $('.Username').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.Username').css('border-color', 'lightgrey');
        }
        //if ($('.Code').val().trim() === "") {
        //    $('.Code').css('border-color', 'Red');
        //    isValid = false;
        //}
        //else {
        //    $('.Code').css('border-color', 'lightgrey');
        //}
        if ($('.Password').val().trim() === "") {
            $('.Password').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.Password').css('border-color', 'lightgrey');
        }
        if ($('.Email').val().trim() === "") {
            $('.Email').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.Email').css('border-color', 'lightgrey');
        }
        if ($('.Alias').val().trim() === "") {
            $('.Alias').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.Alias').css('border-color', 'lightgrey');
        }
       
        return isValid;
    },
    loadData: function (changePageSize) {
        var search = $('#search').val();

        $.ajax({
            url: '/AdminUser/LoadData/',
            type: 'GET',
            data: {
                search: search,
                page: userConfig.pageIndex,
                pageSize: userConfig.pageSize
            },
            dataType: 'json',
            beforeSend: function () {
                $("#main-loading-delay").show();
            },
            success: function (response) {
                var count = 1;
                if (response.status) {
                    $("#main-loading-delay").hide();
                   
                    //console.log(response.data);
                    var data = response.data;
                    let page = response.page;
                    let pageSize = response.pageSize;
                    count = (page - 1) * pageSize;
                    if (page === 1)
                        count = page;
                    else count = (page - 1) * pageSize + 1;
                    var html = '';
                    var template = $('#tbluser-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            Color: count % 2 !== 0 ?"class='info'":"",
                            No: count,
                            Username: item.Username,
                            Password: item.Password,
                            FullName: item.FullName,
                            Code: item.Code,
                            Level: item.LevelID,
                            Email: item.Email,
                            Alias: item.Alias,
                            Skype:item.Skype,
                            IDStatus: item.ID,
                            IsActive: item.IsActive === true ? "" : "checked",
                            Label: item.IsActive === true ? "Unlocked" : "Locked",
                            PermissionName: item.PermissionName,
                            ID: item.ID,
                            IDDelete: item.ID
                        });
                        count++;
                    });
                    $('#tbluser').html(html);
                    adminUserController.paging(response.total, function () {
                        adminUserController.loadData();
                    }, changePageSize);
                    adminUserController.registerEvent();
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / userConfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage === 0 ? 1 : totalPage,
            first: "First",
            next: "Next",
            last: "Last",
            prev: "Previous",
            visiblePages: 10,
            onPageClick: function (event, page) {
                userConfig.pageIndex = page;
                setTimeout(callback, 500);
            }
        });
    }
};
