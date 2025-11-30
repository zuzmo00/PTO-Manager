import Login from "../pages/Login.tsx";
import Landing from "../pages/Landing.tsx";
import Request from "../pages/Request.tsx";
import Deciding from "../pages/Deciding.tsx";
import AcceptedRequestManage from "../pages/AcceptedRequestManage.tsx";
import StatisticsPage from "../pages/StatisticsPage.tsx";
import ManageWorkers from "../pages/ManageWorkers.tsx";
import ManageSpecialDays from "../pages/ManageSpecialDays.tsx";
import ManageDepartment from "../pages/ManageDepartment.tsx";

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
    {
        path: "ManageWorkers",
        component: < ManageWorkers />,
        isPrivate: true
    },
    {
        path: "ManageSpecialDays",
        component: < ManageSpecialDays />,
        isPrivate: true
    },
    {
        path: "ManageDepartment",
        component: < ManageDepartment />,
        isPrivate: true
    },





]