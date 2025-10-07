import api from "../api/api.ts";
import {jwtDecode} from "jwt-decode";
import {
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
    const {token, setToken, nev, setNev, role, setRole, ugyintezoiJogosultsagok, setUgyintezoiJogosultsagok, reszleg, setReszleg} = useContext(AuthContext)

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
                const szerep = decoded[RoleTokenKey];
                const reszleg = decoded[ReszlegTokenKey];

                setToken(token);
                localStorage.setItem(TokenKeyName, token);

                setNev(nev);
                localStorage.setItem(NevKeyName, nev);

                setReszleg(reszleg);
                localStorage.setItem(ReszlegKeyName, reszleg);

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

    return{login, logout, isLoggedIn, token, nev, role, reszleg ,ugyintezoiJogosultsagok}
}

export default useAuth;