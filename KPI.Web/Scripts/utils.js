//Common
//"use strict";
const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,

    timer: 3000,
    customClass: {
        title: 'title-class',
        icon: 'icon-class'
    }
});

const success = function (message) {
    Toast.fire({
        title: message,
        type: 'success'
    });
};


const error = function (message) {
    Toast.fire({
        title: message,
        type: 'error'
    });
};
const warning = function (message) {
    Toast.fire({
        title: message,
        type: 'warning'
    });
};
const info = function (message) {
    Toast.fire({
        title: message,
        type: 'info'
    });
};
const question = function (message) {
    Toast.fire({
        title: message,
        type: 'question'
    });
};
////http
const $post = function (url, data) {
    return new Promise(function (res, rej) {
        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $("#main-loading-delay").show();
            }
        })
        .done(function (data) {
            $("#main-loading-delay").hide();
            res(data);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $("#main-loading-delay").hide();
            rej(textStatus);
        });
    });
};
const $get = function (url, data) {
    return new Promise(function (res, rej) {
        $.ajax({
            type: 'GET',
            url: url,
            data: data,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $("#main-loading-delay").show();
            }
        })
        .done(function (data) {
            res(data);
            $("#main-loading-delay").hide();

        }).fail(function (jqXHR, textStatus, errorThrown) {
            $("#main-loading-delay").hide();
            rej(textStatus);
        });
    });
};

///end http
$(".datepicker.date").datepicker({
    format: 'MM/DD/YYYY',
    icons: {
        time: "fa fa-clock-o",
        date: "fa fa-calendar",
        up: "fa fa-arrow-up",
        down: "fa fa-arrow-down"
    }
});

function periodText(value) {
    switch (value.toLowerCase()) {
        case "w": period = "Weekly"; break;
        case "m": period = "Monthly"; break;
        case "q": period = "Quaterly"; break;
        case "y": period = "Yearly"; break;
        default:
    }
    return period || "";
}
function toTitleCase(str) {
    return str.replace(/(?:^|\s)\w/g, function (match) {
        return match.toUpperCase();
    });
}

function genarateCode(length) {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
}
function getCurrentQuarter(date) {
    var month = date.getMonth() + 1;
    return Math.ceil(month / 3);
}
function getDateOfWeekInYear(dt) {
    var tdt = new Date(dt.valueOf());
    var dayn = (dt.getDay() + 6) % 7;
    tdt.setDate(tdt.getDate() - dayn + 3);
    var firstThursday = tdt.valueOf();
    tdt.setMonth(0, 1);
    if (tdt.getDay() !== 4) {
        tdt.setMonth(0, 1 + ((4 - tdt.getDay()) + 7) % 7);
    }
    return 1 + Math.ceil((firstThursday - tdt) / 604800000);
}
function isInt(n) {
    return Number(n) === n && n % 1 === 0;
}

function isFloat(n) {
    return Number(n) === n && n % 1 !== 0;
}

function toInt(n) {
    var result = Number(n);
    if (isNaN(result))
        return 0;
    else return Number(n);
}


function convertNumberMonthToText(value) {
    switch (value) {
        case 1:
            result = "Jan"; break;
        case 2:
            result = "Feb"; break;
        case 3:
            result = "Mar"; break;
        case 4:
            result = "Apr"; break;
        case 5:
            result = "May"; break;
        case 6:
            result = "Jun"; break;
        case 7:
            result = "Jul"; break;
        case 8:
            result = "Aug"; break;
        case 9:
            result = "Sep"; break;
        case 10:
            result = "Oct"; break;
        case 11:
            result = "Nov"; break;
        case 12:
            result = "Dec"; break;
        default:
            result = "N/A";
            break;
    }
    return result;
}


function activaTab(tab) {
    $('.nav-pills a[href="#' + tab + '"]').tab('show');
}
///Tuan bat dau tu thu 2
function getDateOfISOWeek(w, y) {
    var simple = new Date(y, 0, 1 + (w - 1) * 7);
    var dow = simple.getDay();
    var ISOweekStart = simple;
    var date = simple.getDate();
    var day = simple.getDay();
    if (dow <= 4)
        ISOweekStart.setDate(simple.getDate() - simple.getDay() + 1);
    else
        ISOweekStart.setDate(simple.getDate() + 8 - simple.getDay());
    return getFormattedDate(ISOweekStart);
}
function getEndDateOfISOWeek(w, y) {
    var simple = new Date(y, 0, 1 + (w - 1) * 7);
    var dow = simple.getDay();
    var ISOweekStart = simple;
    if (dow <= 4)
        ISOweekStart.setDate((simple.getDate() - simple.getDay() + 1) + 5);
    else
        ISOweekStart.setDate((simple.getDate() + 8 - simple.getDay()) + 5);
    return getFormattedDate(ISOweekStart);
}
//Dinh dang ngay thang
function getFormattedDate(d) {
    var todayTime = d;
    todayTime = new Date(todayTime);
    var month = todayTime.getMonth() + 1;
    var day = todayTime.getDate();
    var year = todayTime.getFullYear();
    return month + "-" + day + "-" + year;
}

function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
}

function convertDateJson(d) {
    if (d === null)
        d = "/Date(1000000000000)/";

    let milli = d.replace(/\/Date\((-?\d+)\)\//, '$1');
    let date = new Date(parseInt(milli));
    let dd = String(date.getDate()).padStart(2, '0');
    let mm = String(date.getMonth() + 1).padStart(2, '0'); //January is 0!
    let yyyy = date.getFullYear();

    return mm + '/' + dd + '/' + yyyy;
}
function JSONDateWithTime(dateStr) {
    jsonDate = dateStr;
    var d = new Date(parseInt(jsonDate.substr(6)));
    var m, day;
    m = d.getMonth() + 1;
    if (m < 10)
        month = '0' + m;
    else
        month = m;
    if (d.getDate() < 10)
        day = '0' + d.getDate();
    else
        day = d.getDate();

    var year = d.getFullYear();
    var formattedDate = day + "/" + month + "/" + year;
    var hours = d.getHours() < 10 ? "0" + d.getHours() : d.getHours();
    var minutes = d.getMinutes() < 10 ? "0" + d.getMinutes() : d.getMinutes();
    var seconds = d.getSeconds();
    var formattedTime = hours + ":" + minutes;
    formattedDate = formattedTime + " - " + formattedDate;

    //Ngày giờ hiện tại
    var dateObj = new Date();
    var dayNow = dateObj.getDate();
    var monthNow = dateObj.getMonth() + 1;
    var yearNow = dateObj.getFullYear();
    var hoursNow = dateObj.getHours();
    var minutesNow = dateObj.getMinutes();
    var secondsNow = dateObj.getSeconds();

    dateObj = formattedDate;

    if (hoursNow - hours === 0 && minutesNow - minutes === 0 && yearNow - year === 0 && monthNow - month === 0 && dayNow - day === 0) {
        if (secondsNow - seconds === 0)
            dateObj = "just recently";
        else if (secondsNow - seconds === 1)
            dateObj = secondsNow - seconds + " second ago";
        else
            dateObj = secondsNow - seconds + " seconds ago";
    }

    if (hoursNow - hours === 0 && minutesNow - minutes > 0 && yearNow - year === 0 && monthNow - month === 0 && dayNow - day === 0) {


        if (minutesNow - minutes === 1)
            dateObj = minutesNow - minutes + " minute ago";
        else
            dateObj = minutesNow - minutes + " minutes ago";
    }

    if (hoursNow - hours > 0 && yearNow - year === 0 && monthNow - month === 0 && dayNow - day === 0) {
        if (hoursNow - hours === 1)
            dateObj = hoursNow - hours + " hour ago";
        else
            dateObj = hoursNow - hours + " hours ago";
    }

    if (yearNow - year === 0 && monthNow - month === 0 && dayNow - day > 0 && dayNow - day <= 7) {
        if (dayNow - day === 1)
            dateObj = dayNow - day + " day ago";
        else
            dateObj = dayNow - day + " days ago";
    }

    if (yearNow - year === 0 && monthNow !== month) {
        day = dayNow - day;
        var month = monthNow - month;
        day = month * 30 + day;
        if (0 < day && day <= 7) {
            dateObj = day + " days ago";
        }

    }

    return dateObj;
}


//function getCurrentQuarter(d) {
//    d = d || new Date();
//    var m = Math.floor(d.getMonth() / 3) + 2;
//    return m > 4 ? m - 4 : m;
//}
//function getDateOfWeekInYear(dt) {
//    var tdt = new Date(dt.valueOf());
//    var dayn = (dt.getDay() + 6) % 7;
//    tdt.setDate(tdt.getDate() - dayn + 3);
//    var firstThursday = tdt.valueOf();
//    tdt.setMonth(0, 1);
//    if (tdt.getDay() !== 4) {
//        tdt.setMonth(0, 1 + ((4 - tdt.getDay()) + 7) % 7);
//    }
//    return 1 + Math.ceil((firstThursday - tdt) / 604800000);
//}
function convertDate(inputFormat) {
    function pad(s) { return s < 10 ? '0' + s : s; }
    var d = new Date(inputFormat);
    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/');
}

function dateNow() {
    var date = new Date();
    var day = date.getDate();       // yields date
    var month = date.getMonth() + 1;    // yields month (add one as '.getMonth()' is zero indexed)
    var year = date.getFullYear();  // yields year
    var hour = date.getHours();     // yields hours 
    var minute = date.getMinutes(); // yields minutes
    var second = date.getSeconds(); // yields seconds

    // After this construct a string with the above results as below
    var time = day + "/" + month + "/" + year + " " + hour + ':' + minute + ':' + second;
    return time;
}


function Binding(b) {
    _this = this;
    this.elementBindings = [];
    this.value = b.object[b.property];
    this.valueGetter = function () {
        return _this.value;
    };
    this.valueSetter = function (val) {
        _this.value = val;
        for (var i = 0; i < _this.elementBindings.length; i++) {
            var binding = _this.elementBindings[i];
            binding.element[binding.attribute] = val;
        }
    }
    this.addBinding = function (element, attribute, event) {
        var binding = {
            element: element,
            attribute: attribute
        };
        if (event) {
            element.addEventListener(event, function (event) {
                _this.valueSetter(element[attribute]);
            });
            binding.event = event;
        }
        this.elementBindings.push(binding);
        element[attribute] = _this.value;
        return _this;
    };

    Object.defineProperty(b.object, b.property, {
        get: this.valueGetter,
        set: this.valueSetter
    });

    b.object[b.property] = this.value;
}