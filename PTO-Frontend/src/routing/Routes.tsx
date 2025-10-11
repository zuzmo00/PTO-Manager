import Login from "../pages/Login.tsx";
import Landing from "../pages/Landing.tsx";

export const routes = [
    {
        path: "login",
        component: < Login />,
        isPrivate: false
    },
    {
        path: "Landing",
        component: < Landing />,
        isPrivate: false
    },

]