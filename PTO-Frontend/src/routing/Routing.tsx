import {routes} from "./Routes";
import type {ReactElement} from "react";
import {Route, Routes, Navigate} from "react-router-dom";
import useAuth from "../hooks/useAuth.tsx";
import BasicLayout from "../components/layout/BasicLayout.tsx";

//Ha valaki nincs regisztrálva akkor redirect a loginra
const PrivateRoute = ({element}: {element: ReactElement}) => {
    const {isLoggedIn} = useAuth();
    return isLoggedIn ? element : <Navigate to="/login"/>;
};

//Ez azért kell, hogy egy bejelentkezett felhasználó ha mondjuk a logint akarja elérni akkor redirectelve legyen
const AuthenticatedRedirect = ({element} : {element: ReactElement}) =>{
    const {isLoggedIn} = useAuth();
    return isLoggedIn ?<Navigate to="/app"/> : element;
};



const Routing = () => {

    //Ez a rész felel azért, ha valaki a rootot akarja elérni akkor eldönti,hova redirectelje
    return<Routes>
        <Route
            path="/"
            element={<AuthenticatedRedirect element={<Navigate to="login" />}/>}
        />
        {
            routes.filter(route => !route.isPrivate).map(route => (
                <Route
                    key={route.path}
                    path={route.path}
                    element={<AuthenticatedRedirect element={route.component}/>}
                />
            ))
        }
        <Route
            path="app"
            element={<PrivateRoute element={<BasicLayout/>}/>}>
            <Route
                path=""
                element={<Navigate to="dashboard" />}
            />
            {
                routes.filter(route=> route.isPrivate).map(route=> (
                    <Route
                        key={route.path}
                        path={route.path}
                        element={<PrivateRoute element={route.component}/>}
                    />
                ))
            }
        </Route>
    </Routes>

}
export default Routing;