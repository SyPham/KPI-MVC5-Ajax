
$(document).ready(function () {
    $('table tr').addClass("text-center").css("height", "50px").css("line-height", "50px");
    $('table th').addClass("text-center")
    $('table tr td a').addClass('text-black');

    $('table tr td.kpi-name').click(function () {
        window.location = $(this).attr('href');
        return false;
    });

    $('table tr td i').click(function () {
        $(this).toggleClass('text-red');
    });
    favouriteController.init();
});
var config = {
    pageSize: 6,
    pageIndex: 1
};
var favouriteController = {
    init: function () {
        favouriteController.loadData();
        favouriteController.registerEvent();
    },
    registerEvent: function () {
        
        $('.del').off('click').on('click', function () {
            var id = $(this).data('id');
            favouriteController.delete(id);
            favouriteController.loadData();
           
        });
    },
    delete: function (id) {
        $.ajax({
            url: '/Favourite/Delete',
            type: 'POST',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (res) {
                if (res) {
                    success('Delete successfully!');
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
    ,
    loadData: function (changePageSize) {
        var userid = $('#user').data('userid');

        $.ajax({
            url: '/Favourite/LoadData',
            type: 'GET',
            data: {
                userid: userid,
                page: config.pageIndex,
                pageSize: config.pageSize
            },
            dataType: 'json',
            beforeSend: function () {
                $("#main-loading-delay").show();
            },
            success: function (response) {
                console.log(response);
                if (response.status) {
                    $("#main-loading-delay").hide();
                    var count = 1;
                    //console.log(response.data);
                    var data = response.data;
                    var html = '';
                    var template = $('#tblfavourite-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            No: count,
                            KPIName: item.KPIName,
                            KPILevelCode: item.KPILevelCode,
                            Period: item.Period,
                            //Status: item.Status,
                            //Remark: item.Remark,
                            User: item.Username,
                            Team: item.TeamName,
                            Level: item.Level,
                            ID: item.ID
                        });
                        count++;
                    });
                    $('#tblfavourite').html(html);
                    favouriteController.paging(response.total, function () {
                        favouriteController.loadData();
                    }, changePageSize);
                    favouriteController.registerEvent();
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / config.pageSize);

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
                config.pageIndex = page;
                setTimeout(callback, 500);
            }
        });
    }
};
