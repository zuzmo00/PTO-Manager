import "@mantine/core/styles.css";
import '@mantine/dates/styles.css'
import '@mantine/charts/styles.css';
import {useState} from "react";
import {
    EmailKeyName,
    NameKeyName,
    DepartmentKeyName,
    RoleKeyName,
    TokenKeyName,
    AdminPrivilegesKeyName
} from "./constants/constants.ts";
import {MantineProvider} from "@mantine/core";
import {BrowserRouter} from "react-router-dom";
import { AuthContext } from "./context/AuthContext.tsx";
import Routing from "./routing/Routing.tsx";
import {Notifications} from "@mantine/notifications";
import {theme} from "./theme.ts";


function App() {

    const [token, setToken] = useState(localStorage.getItem(TokenKeyName));
    const [nev, setNev] = useState(localStorage.getItem(NameKeyName));
    const [reszleg,setReszleg] = useState(localStorage.getItem(DepartmentKeyName));
    const [role, setRole] = useState(localStorage.getItem(RoleKeyName));
    const [email, setEmail] = useState(localStorage.getItem(EmailKeyName));
    const [ugyintezoiJogosultsagok, setUgyintezoiJogosultsagok] = useState(() => {
        const raw = localStorage.getItem(AdminPrivilegesKeyName);
        return raw ? JSON.parse(raw) : null;
    });


    return <MantineProvider theme={theme}>
        <Notifications />
            <BrowserRouter>
                <AuthContext.Provider value={{token, setToken,nev, setReszleg, reszleg, setNev, role, setRole,ugyintezoiJogosultsagok, setUgyintezoiJogosultsagok, email, setEmail}}>
                    <Routing/>
                </AuthContext.Provider>
            </BrowserRouter>
    </MantineProvider>;


}
export default App
