import "@mantine/core/styles.css";
import {useState} from "react";
import {
    NevKeyName,
    ReszlegKeyName,
    RoleKeyName,
    TokenKeyName,
    UgyintezoiJogosultsagokKeyName
} from "./constants/constants.ts";
import {MantineProvider} from "@mantine/core";
import {BrowserRouter} from "react-router-dom";
import { AuthContext } from "./context/AuthContext.tsx";
import Routing from "./routing/Routing.tsx";
import {Notifications} from "@mantine/notifications";
import {theme} from "./theme.ts";


function App() {

    const [token, setToken] = useState(localStorage.getItem(TokenKeyName));
    const [nev, setNev] = useState(localStorage.getItem(NevKeyName));
    const [reszleg,setReszleg] = useState(localStorage.getItem(ReszlegKeyName));
    const [role, setRole] = useState(localStorage.getItem(RoleKeyName));
    const [ugyintezoiJogosultsagok, setUgyintezoiJogosultsagok] = useState(() => {
        const raw = localStorage.getItem(UgyintezoiJogosultsagokKeyName);
        return raw ? JSON.parse(raw) : null;
    });


    return <MantineProvider theme={theme}>
        <Notifications />
            <BrowserRouter>
                <AuthContext.Provider value={{token, setToken,nev, setReszleg, reszleg, setNev, role, setRole,ugyintezoiJogosultsagok, setUgyintezoiJogosultsagok}}>
                    <Routing/>
                </AuthContext.Provider>
            </BrowserRouter>
    </MantineProvider>;


}
export default App
