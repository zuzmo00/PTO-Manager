import api from "../api/api.ts";
import {jwtDecode} from "jwt-decode";
import {
    EmailKeyName, EmailTokenKey,
    NameKeyName,
    NameTokenKey,
    DepartmentKeyName,
    DepartmentTokenKey, RoleKeyName,
    RoleTokenKey,
    TokenKeyName, AdminPrivilegesKeyName
} from "../constants/constants.ts";
import {useContext} from "react";
import {AuthContext} from "../context/AuthContext.tsx";


const useAuth = () => {
    const {token, setToken, nev, setNev, role, setRole, ugyintezoiJogosultsagok, setUgyintezoiJogosultsagok, reszleg, setReszleg, email, setEmail} = useContext(AuthContext)

    const isLoggedIn = !!token;

    const login = async (email: string, jelszo: string) => {
        // eslint-disable-next-line no-useless-catch
        try{
            const response = await api.Auth.login({email: email, jelszo: jelszo});

            if (response.data.success && response.data.data) {
                const token = response.data.data.token;
                const privileges = response.data.data.adminPrivileges;


                const decoded: any = jwtDecode(token);
                const nev = decoded[NameTokenKey];
                const email = decoded[EmailTokenKey];
                const szerep = decoded[RoleTokenKey];
                const reszleg = decoded[DepartmentTokenKey];

                setToken(token);
                localStorage.setItem(TokenKeyName, token);

                setNev(nev);
                localStorage.setItem(NameKeyName, nev);

                setReszleg(reszleg);
                localStorage.setItem(DepartmentKeyName, reszleg);

                setEmail(email);
                localStorage.setItem(EmailKeyName, email);

                setRole(szerep);
                localStorage.setItem(RoleKeyName, szerep);

                setUgyintezoiJogosultsagok(privileges);
                localStorage.setItem(AdminPrivilegesKeyName, JSON.stringify(privileges));
            }else {
                throw new Error(response.data.message || "Ismeretlen hiba");
            }
        }catch (error) {
            throw error;
        }
    }
    const logout=() => {
        localStorage.clear();
        setToken(null);
    }

    return{login, logout, isLoggedIn, token, nev, role, reszleg ,ugyintezoiJogosultsagok, email}
}

export default useAuth;