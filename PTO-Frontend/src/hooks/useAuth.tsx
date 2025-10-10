import api from "../api/api.ts";
import {jwtDecode} from "jwt-decode";
import {
    EmailKeyName, EmailTokenKey,
    NevKeyName,
    NevTokenKey,
    ReszlegKeyName,
    ReszlegTokenKey, RoleKeyName,
    RoleTokenKey,
    TokenKeyName, UgyintezoiJogosultsagokKeyName
} from "../constants/constants.ts";
import {useContext} from "react";
import {AuthContext} from "../context/AuthContext.tsx";


const useAuth = () => {
    const {token, setToken, nev, setNev, role, setRole, ugyintezoiJogosultsagok, setUgyintezoiJogosultsagok, reszleg, setReszleg, email, setEmail} = useContext(AuthContext)

    const isLoggedIn = !!token;

    const login = async (email: string, jelszo: string) => {
        // eslint-disable-next-line no-useless-catch
        try{
            const response = await api.Auth.login({Email: email, Jelszo: jelszo});

            if (response.data.success && response.data.data) {
                const token = response.data.data.token;
                const jogosultsagok = response.data.data.ugyintezoiJogosultsagok;


                const decoded: any = jwtDecode(token);
                const nev = decoded[NevTokenKey];
                const email = decoded[EmailTokenKey];
                const szerep = decoded[RoleTokenKey];
                const reszleg = decoded[ReszlegTokenKey];

                setToken(token);
                localStorage.setItem(TokenKeyName, token);

                setNev(nev);
                localStorage.setItem(NevKeyName, nev);

                setReszleg(reszleg);
                localStorage.setItem(ReszlegKeyName, reszleg);

                setEmail(email);
                localStorage.setItem(EmailKeyName, email);

                setRole(szerep);
                localStorage.setItem(RoleKeyName, szerep);

                setUgyintezoiJogosultsagok(jogosultsagok);
                localStorage.setItem(UgyintezoiJogosultsagokKeyName, JSON.stringify(jogosultsagok));
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