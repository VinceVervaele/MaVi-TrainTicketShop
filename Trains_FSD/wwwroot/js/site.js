/* active link tracking */
$(document).ready(function () {
    const currentUrlPath = getCurrentUrlPath();
    setActiveLink(currentUrlPath);
    setActiveCartLink(currentUrlPath);
});

const getCurrentUrlPath = () => {
    return window.location.pathname;
};

const setActiveLink = (currentUrlPath) => {
    const homeLink = $("#home-link");
    const ticketsLink = $("#tickets-link");
    const aboutLink = $("#about-link");
    const cartLink = $("#cart-link");

    if (currentUrlPath === "/") {
        homeLink.addClass("active");
    }
    else if (currentUrlPath.startsWith("/Ticket/TicketShop")) {
        ticketsLink.addClass("active");
    }
    else if (currentUrlPath.startsWith("/Home/Privacy")) {
        aboutLink.addClass("active");
    }
    else if (currentUrlPath.startsWith("/ShoppingCart")
        | currentUrlPath.startsWith("/Order/GetAllOrders")
        | currentUrlPath.startsWith("/Order/GetPersonalOrders")) {
        cartLink.addClass("active");
    }
    else { }
};

const setActiveCartLink = (currentUrlPath) => {
    const cartList = $("#cartList");
    const cartHistory = $("#cartHistory");
    const cartPersonal = $("#cartPersonal");

    if (currentUrlPath.startsWith("/ShoppingCart")) {
        cartList.addClass("activeCartNav");
    } else if (currentUrlPath.startsWith("/Order/GetAllOrders")) {
        cartHistory.addClass("activeCartNav");
    } else if (currentUrlPath.startsWith("/Order/GetPersonalOrders")) {
        cartPersonal.addClass("activeCartNav");
    }
    else { }
};
/* end active link tracking */
