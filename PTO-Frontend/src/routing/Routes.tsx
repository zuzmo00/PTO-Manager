import Login from "../pages/Login.tsx";
import Landing from "../pages/Landing.tsx";
import Request from "../pages/Request.tsx";
import Deciding from "../pages/Deciding.tsx";
import AcceptedRequestManage from "../pages/AcceptedRequestManage.tsx";
import StatisticsPage from "../pages/StatisticsPage.tsx";

export const routes = [
    {
        path: "login",
        component: < Login />,
        isPrivate: false
    },
    {
        path: "Landing",
        component: < Landing />,
        isPrivate: true
    },
    {
        path: "Request",
        component: < Request />,
        isPrivate: true
    },
    {
        path: "Deciding",
        component: < Deciding />,
        isPrivate: true
    },
    {
        path: "AcceptedRequestManage",
        component: < AcceptedRequestManage />,
        isPrivate: true
    },
    {
        path: "StatisticsPage",
        component: < StatisticsPage />,
        isPrivate: true
    },




]