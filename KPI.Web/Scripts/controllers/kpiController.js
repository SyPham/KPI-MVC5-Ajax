$(document).ready(function () {
    kpiController.init();
});
var kpiLevelConfig = {
    pageSize: 6,
    pageIndex: 1
};
var kpiController = {
    init () {
        kpiController.loadData();
        kpiController.registerEvent();
    },
    registerEvent () {
        $('#search').off('keypress').on('keypress', function (e) {
            if (e.which === 13) {
                kpiController.loadData(true);
            }
        });
        $('#box select').off('change').on('change', function (e) {
            var code = $(this).parent().children('.code').text();
            kpiController.loadDataKPILevel(true, code);
        });
        //Save KPI
        $('#btnAdd').off('click').on('click', function () {
            kpiController.addData();

        });
        //Delete
        $('#btnDelete').off('click').on('click', function () {

        });
        //Update
        $('.btnUpdate').off('click').on('click', function () {
            //load detail
            kpiController.loadDetail(Number($(this).data('id')));
        });
        //save
        $('#btnSaveUpdateModal').off('click').on('click', function () {
            kpiController.updateData();
        });
        //delete
        $('.btnDelete').off('click').on('click', function () {
            kpiController.deleteData(Number($(this).data('id')));
        });
        //get kpicode
        $(".get-kpi-code").click(function () {

            var code = $('#box .kpi-name .code').text();
            var level = $('#box .kpi-name .level').text();

            var codekpi = $(this).parent('td').parent('tr').children('td:nth-child(2)').children('div').children('input').val();
            $('#updateKpi input').val(level + code + 'POM' + codekpi);

        });
        //showkpi Modal
        $('#tbldepartment .showTblKPI').off('click').on('click', function () {
            $('#box').toggle(800);
            $('#box .kpi-name h3').text('KPI - ' + $(this).closest('td').closest('tr').find('td:nth-child(2)').text());
            $('#box .kpi-name .code').text($(this).closest('td').closest('tr').find('td:nth-child(3)').text());
            $('#box .kpi-name .level').text($(this).closest('td').closest('tr').find('td:nth-child(4)').text());
            $('#box .kpi-name .id').text($(this).closest('td').closest('tr').find('td:first-child').data('id'));
            var code = $(this).parent().parent().parent().children('td:nth-child(3)').text();
            kpiController.loadDataKPILevel(true, code);
            kpiController.loadDataCategory();
        });
        //update kpilevel
        $('#tblkpilevel tr td:nth-child(2) input').change(function () {
            var codekpi = $('#box .kpi-name .code').text();
            var level = $('#box .kpi-name .level').text();
            var KPICode = $(this).val();
            var TableID = $('#box .kpi-name .code').text();
            var TimeCheck = new Date();
            var CodeKPILevel = level + codekpi + 'POM' + codekpi;
            var Checked = $(this).is(':checked');
            var UserCheck = 1;
            var Weekly = "";
            var Monthly = "2001/01/01";
            var Quaterly = "2001/01/01";
            var Yearly = "2001/01/01";
            var WeeklyChecked = null;
            var MonthlyChecked = null;
            var QuaterlyChecked = null;
            var YearlyChecked = null;
            kpiController.updateKPILevel(KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID);
        });

        //----------------------------------------------------------------------------------------------

        //update weeklychecked kpilevel
        $('#tblkpilevel .weekly').off('click').on('click', function () {

            var codekpi = $('#box .kpi-name .code').text();
            var level = $('#box .kpi-name .level').text();
            var KPICode = $(this).parent().parent().parent().find('td:nth-child(2) input').val();
            var TableID = $('#box .kpi-name .code').text();
            var TimeCheck = new Date();
            var CodeKPILevel = level + codekpi + 'POM' + codekpi;
            var Checked = null;
            var UserCheck = 1;
            var Weekly = "";
            var Monthly = "2001/01/01";
            var Quaterly = "2001/01/01";
            var Yearly = "2001/01/01";
            var WeeklyChecked = $(this).is(':checked');
            var MonthlyChecked = null;
            var QuaterlyChecked = null;
            var YearlyChecked = null;
            kpiController.updateKPILevel(KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID);
            if ($(this).is(':checked')) {
                $('#modal-group-weekly').modal('show').fadeIn(1000);
                $('#modal-group-weekly select').attr('data-id', KPICode);
            }

        });
        //update monthly checked kpilevel
        $('#tblkpilevel .monthly').off('click').on('click', function () {

            var codekpi = $('#box .kpi-name .code').text();
            var level = $('#box .kpi-name .level').text();
            var KPICode = $(this).parent().parent().parent().find('td:nth-child(2) input').val();
            var TableID = $('#box .kpi-name .code').text();
            var TimeCheck = new Date();
            var CodeKPILevel = level + codekpi + 'POM' + codekpi;
            var Checked = null;
            var UserCheck = 1;
            var Weekly = "";
            var Monthly = "2001/01/01";
            var Quaterly = "2001/01/01";
            var Yearly = "2001/01/01";
            var WeeklyChecked = null;
            var MonthlyChecked = $(this).is(':checked');
            var QuaterlyChecked = null;
            var YearlyChecked = null;
            kpiController.updateKPILevel(KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID);
            if ($(this).is(':checked')) {
                $('#modal-group-monthly').modal('show').fadeIn(1000);
                $('#modal-group-monthly .monthly').attr('data-id', KPICode);
            }
        });
        //update quaterly checked kpilevel
        $('#tblkpilevel .quaterly').off('click').on('click', function () {

            var codekpi = $('#box .kpi-name .code').text();
            var level = $('#box .kpi-name .level').text();
            var KPICode = $(this).parent().parent().parent().find('td:nth-child(2) input').val();
            var TableID = $('#box .kpi-name .code').text();
            var TimeCheck = new Date();
            var CodeKPILevel = level + codekpi + 'POM' + codekpi;
            var Checked = null;
            var UserCheck = 1;
            var Weekly = "";
            var Monthly = "2001/01/01";
            var Quaterly = "2001/01/01";
            var Yearly = "2001/01/01";
            var WeeklyChecked = null;
            var MonthlyChecked = null;
            var QuaterlyChecked = $(this).is(':checked');
            var YearlyChecked = null;
            kpiController.updateKPILevel(KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID);
            if ($(this).is(':checked')) {
                $('#modal-group-quaterly').modal('show').fadeIn(1000);
                $('#modal-group-quaterly .quaterly').attr('data-id', KPICode);
            }
        });
        //update yearly checked kpilevel
        $('#tblkpilevel .yearly').off('click').on('click', function () {
            var codekpi = $('#box .kpi-name .code').text();
            var level = $('#box .kpi-name .level').text();
            var KPICode = $(this).parent().parent().parent().find('td:nth-child(2) input').val();
            var TableID = $('#box .kpi-name .code').text();
            var TimeCheck = new Date();
            var CodeKPILevel = level + codekpi + 'POM' + codekpi;
            var Checked = null;
            var UserCheck = 1;
            var Weekly = "";
            var Monthly = "2001/01/01";
            var Quaterly = "2001/01/01";
            var Yearly = "2001/01/01";
            var WeeklyChecked = null;
            var MonthlyChecked = null;
            var QuaterlyChecked = null;
            var YearlyChecked = $(this).is(':checked');
            kpiController.updateKPILevel(KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID);
            if ($(this).is(':checked')) {
                $('#modal-group-yearly').modal('show').fadeIn(1000);
                $('#modal-group-yearly .yearly').attr('data-id', KPICode);
            }
        });

        //----------------------------------------------------------------------------------------------

        //update weekly modal
        $('#btnsaveweekly').off('click').on('click', function () {
            var codekpi = $('#box .kpi-name .code').text();
            var level = $('#box .kpi-name .level').text();
            var KPICode = $('#modal-group-weekly .weekly').attr('data-id');
            var TableID = $('#box .kpi-name .code').text();
            var TimeCheck = new Date();
            var CodeKPILevel = level + codekpi + 'POM' + codekpi;
            var Checked = null;
            var UserCheck = 1;
            var Weekly = $("#modal-group-weekly .weekly").val();
            var Monthly = $(this).val();
            var Quaterly = "2001/01/01";
            var Yearly = "2001/01/01";
            var WeeklyChecked = null;
            var MonthlyChecked = null;
            var QuaterlyChecked = null;
            var YearlyChecked = null;
            kpiController.updateKPILevel(KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID);

        });
        //update monthly modal
        $("#btnsavemonthlymodal").off('click').on('click', function (e) {

            e.preventDefault();
            var codekpi = $('#box .kpi-name .code').text();
            var level = $('#box .kpi-name .level').text();
            var KPICode = $('#modal-group-monthly .monthly').data('id');
            var TableID = $('#box .kpi-name .code').text();
            var TimeCheck = new Date();
            var CodeKPILevel = level + codekpi + 'POM' + codekpi;
            var Checked = null;
            var UserCheck = 1;
            var Weekly = "";
            var Monthly = $("#modal-group-monthly .monthly input").val();
            var Quaterly = "2001/01/01";
            var Yearly = "2001/01/01";
            var WeeklyChecked = null;
            var MonthlyChecked = null;
            var QuaterlyChecked = null;
            var YearlyChecked = null;
            kpiController.updateKPILevel(KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID);


        });
        //update quaterly modal
        $("#btnsavequaterlymodal").off('click').on('click', function (e) {
            e.preventDefault();
            var codekpi = $('#box .kpi-name .code').text();
            var level = $('#box .kpi-name .level').text();
            var KPICode = $('#modal-group-quaterly .quaterly').data('id');
            var TableID = $('#box .kpi-name .code').text();
            var TimeCheck = new Date();
            var CodeKPILevel = level + codekpi + 'POM' + codekpi;
            var Checked = null;
            var UserCheck = 1;
            var Weekly = "";
            var Monthly = "2001/01/01";
            var Quaterly = new Date($("#modal-group-quaterly .quaterly input").val());
            var Yearly = "2001/01/01";
            var WeeklyChecked = null;
            var MonthlyChecked = null;
            var QuaterlyChecked = null;
            var YearlyChecked = null;

            setTimeout(kpiController.updateKPILevel(KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID)
                , 200);

        });
        //update yearly modal
        $("#btnsaveyearly").off('click').on('click', function () {


            var codekpi = $('#box .kpi-name .code').text();
            var level = $('#box .kpi-name .level').text();
            var KPICode = $('#modal-group-yearly .yearly').data('id');
            var TableID = $('#box .kpi-name .code').text();
            var TimeCheck = new Date();
            var CodeKPILevel = level + codekpi + 'POM' + codekpi;
            var Checked = null;
            var UserCheck = 1;
            var Weekly = "";
            var Monthly = "2001/01/01";
            var Quaterly = "2001/01/01";
            var Yearly = $("#modal-group-yearly .yearly input").val();
            var WeeklyChecked = null;
            var MonthlyChecked = null;
            var QuaterlyChecked = null;
            var YearlyChecked = null;
            kpiController.updateKPILevel(KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID);


        });
    },
    addData () {
        var res = kpiController.validate();
        if (res === false) {
            return false;
        }
        var mObj = {
            Code: $('#addKPI .Code').val(),
            Name: $('#addKPI .Name').val(),
            LevelID: $('#addKPI .LevelID').val()
        };
        $.ajax({
            url: "/AdminDepartment/Add",
            data: JSON.stringify(mObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result === 1) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Add successfully!',
                        type: 'success',
                        confirmButtonText: 'OK'
                    });
                    $("#modal-group").modal('hide');
                    kpiController.loadData(true);
                    kpiController.resetForm();
                }
                else {
                    Swal.fire({
                        title: 'Error!',
                        text: 'This code has already existed!',
                        type: 'error'
                    });
                }

            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    },
    deleteData (id) {
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
                    url: "/AdminDepartment/Delete/" + ID,
                    type: "POST",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        Swal.fire({
                            title: 'Success!',
                            text: 'Delete successfully.',
                            type: 'success'
                        });
                        kpiController.loadData();
                        $("#modal-group").modal('hide');
                    },
                    error: function (errormessage) {
                        console.log(errormessage.responseText);
                    }
                });
                Swal.fire(
                    'Deleted!',
                    'Reacord has been deleted.',
                    'success'
                );
            }
        });

    },
    resetForm () {
        $('.ID').val("");
        $('.Name').val("");
        $('.Code').val("");
        $('.LevelID').val("");
        $('.Name').css('border-color', 'lightgrey');
        $('.Code').css('border-color', 'lightgrey');
        $('.LevelID').css('border-color', 'lightgrey');
    },
    updateData (id, name, code, levelID) {
        var mObj = {
            ID: $('#updateKpi .ID').val(),
            Name: $('#updateKpi .Name').val(),
            Code: $('#updateKpi .Code').val(),
            LevelID: $('#updateKpi .LevelID').val()

        };

        $.ajax({
            url: "/AdminDepartment/Update",
            data: JSON.stringify(mObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Update successfully!',
                        type: 'success',
                    });
                    kpiController.resetForm();
                    $("#modal-group2").modal('hide');
                    kpiController.loadData();

                }

            },
            error: function (error) {
                console.log(error);
            }
        });
    },
    updateKPILevel (KPICode, CodeKPILevel, UserCheck, Checked, WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked, Weekly, Monthly, Quaterly, Yearly, TimeCheck, TableID) {
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
            url: "/AdminDepartment/UpdateKPILevel",
            data: JSON.stringify(mObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result) {

                    Swal.fire({
                        title: 'Success!',
                        text: 'Update successfully!',
                        type: 'success'
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
            url: "/AdminDepartment/GetbyID/",
            data: { ID: JSON.stringify(value) },
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                $('#updateKpi .ID').val(result.ID);
                $('#updateKpi .Name').val(result.Name);
                $('#updateKpi .Code').val(result.Code);
                $('#updateKpi .LevelID').val(result.LevelID);

                $('#modal-group2').modal('show');

            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadDataCategory: function () {
        $.ajax({
            url: '/AdminKPI/GetCategoryCode',
            type: "GET",
            dataType: "json",
            success: function (data) {
                $("#box select").empty();
                $("#box select").append('<option value="">.: Choose Category :.</option>');
                //console.log(data);
                $.each(data, function (key, item) {
                    $("#box select").append("<option value=\"" + item.Code + "\">" + item.Name + "</option>");
                });
            }
        });
    },
    loadDataKPILevel: function (changePageSize, code) {
        var category = $('#box select').find(':selected').val();
        $.ajax({
            url: '/AdminDepartment/LoadDataKPILevel',
            type: "GET",
            data: {
                code: code,
                category: category,
                page: kpiLevelConfig.pageIndex,
                pageSize: kpiLevelConfig.pageSize
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var count = 1;
                    var data = response.data;
                    var html = '';
                    var template = $('#tblkpilevel-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            No: count,
                            KPICode: item.KPICode,
                            Name: item.Name,
                            Checked: item.Checked === true ? '<input type="checkbox" value="' + item.KPICode + '" checked />' : '<input type="checkbox" value="' + item.KPICode + '" />',
                            WeeklyChecked: item.WeeklyChecked === true ? '<input class="weekly" value="' + item.KPICode + '" type="checkbox" checked />' : '<input class="weekly" value="' + item.KPICode + '" type="checkbox" />',
                            MonthlyChecked: item.MonthlyChecked === true ? '<input class="monthly" value="' + item.KPICode + '" type="checkbox" checked />' : '<input class="monthly" value="' + item.KPICode + '" type="checkbox" />',
                            QuaterlyChecked: item.QuaterlyChecked === true ? '<input class="quaterly" value="' + item.KPICode + '" type="checkbox" checked />' : '<input class="quaterly" value="' + item.KPICode + '" type="checkbox" />',
                            YearlyChecked: item.YearlyChecked === true ? '<input class="yearly"  value="' + item.KPICode + '"type="checkbox" checked />' : '<input class="yearly" value="' + item.KPICode + '" type="checkbox" />',

                        });
                        count++;
                    });
                    $('#tblkpilevel').html(html);
                    kpiController.pagingKPILevel(response.total, function () {
                        kpiController.loadDataKPILevel("", code);
                    }, changePageSize);
                    kpiController.registerEvent();
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    pagingKPILevel: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / kpiLevelConfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationKPILevel a').length === 0 || changePageSize === true) {
            $('#paginationKPILevel').empty();
            $('#paginationKPILevel').removeData("twbs-pagination");
            $('#paginationKPILevel').unbind("page");
        }

        $('#paginationKPILevel').twbsPagination({
            totalPages: totalPage === 0 ? 1 : totalPage,
            first: "First",
            next: "Next",
            last: "Last",
            prev: "Previous",
            visiblePages: 10,
            onPageClick: function (event, page) {
                kpiLevelConfig.pageIndex = page;
                setTimeout(callback, 500);
            }
        });
    },
    validate: function () {
        var isValid = true;
        if ($('.Name').val().trim() === "") {
            $('.Name').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.Name').css('border-color', 'lightgrey');
        }
        if ($('.Code').val().trim() === "") {
            $('.Code').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.Code').css('border-color', 'lightgrey');
        }
        if ($('.LevelID').val().trim() === "") {
            $('.LevelID').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.LevelID').css('border-color', 'lightgrey');
        }
        return isValid;
    },
    loadData: function (changePageSize) {
        var search = $('#search').val();

        $.ajax({
            url: '/AdminDepartment/LoadData/',
            type: 'GET',
            data: {
                search: search,
                page: departmentConfig.pageIndex,
                pageSize: departmentConfig.pageSize
            },
            dataType: 'json',
            beforeSend: function () {
                $("#main-loading-delay").show();
            },
            success: function (response) {
                if (response.status) {
                    $("#main-loading-delay").hide();
                    var count = 1;
                    //console.log(response.data);
                    var data = response.data;
                    var html = '';
                    var template = $('#tbldepartment-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            No: count,
                            Name: item.Name,
                            Code: item.Code,
                            Level: item.LevelID,
                            ID: item.ID,
                            IDDelete: item.ID
                        });
                        count++;
                    });
                    $('#tbldepartment').html(html);
                    kpiController.paging(response.total, function () {
                        kpiController.loadData();
                    }, changePageSize);
                    kpiController.registerEvent();
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / departmentConfig.pageSize);

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
                departmentConfig.pageIndex = page;
                setTimeout(callback, 500);
            }
        });
    }
};
