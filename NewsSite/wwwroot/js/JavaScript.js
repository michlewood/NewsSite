$(function () {
    $.ajax({
        url: '/check/view/getAll',
        method: 'GET'
    })
        .done(function (result) {
            $.each(result, function (index, item) {
                $("#selectUser").append(`<option value="${item.email}">${item.email}</option>`);
            });
        });
});

$("#Login").click(function () {
    let username = $("#selectUser").val();

    $.ajax({
        url: '/check/view/login',
        method: 'POST',
        data: { email: username }
    })
        .done(function (result) {
            console.log(`logged in: ${result}`);
        });
});

$("#RecoverUsers").click(function () {

    $.ajax({
        url: '/check/view/Recover',
        method: 'Get'
    })
        .done(function (result) {
            console.log(result);
        });
});

$("#ShowAllUsersWithClaims").click(function () {

    $.ajax({
        url: '/check/view/getAllWithClaims',
        method: 'GET'
    })
        .done(function (result) {
            console.log(result);
        });
});

$("#checkCanSeeOpen").click(function () {
    $.ajax({
        url: '/check/view/open',
        method: 'GET'
    })
        .done(function (result) {
            console.log(result);
        })
        .fail(function (xhr, status, error) {
            console.log("Error", xhr, status, error);
        });
});

$("#checkCanSeeHidden").click(function () {
    $.ajax({
        url: '/check/view/hidden',
        method: 'GET'
    })
        .done(function (result) {
            console.log(result);
        })
        .fail(function (xhr, status, error) {
            console.log("Error", xhr, status, error);
        });
});

$("#checkCanSeeHiddenAndIs20Years").click(function () {
    $.ajax({
        url: '/check/view/hiddenAndAge',
        method: 'GET'
    })
        .done(function (result) {

            console.log(result);
        })
        .fail(function (xhr, status, error) {
            console.log("Error", xhr, status, error);
        });
});

$("#checkCanPublishSports").click(function () {
    $.ajax({
        url: '/check/view/sport',
        method: 'GET'
    })
        .done(function (result) {

            console.log(result);
        })
        .fail(function (xhr, status, error) {
            console.log("Error", xhr, status, error);
        });
});

$("#checkCanPublishCulture").click(function () {
    $.ajax({
        url: '/check/view/culture',
        method: 'GET'
    })
        .done(function (result) {
            console.log(result);
        })
        .fail(function (xhr, status, error) {
            console.log("Error", xhr, status, error);
        });
});