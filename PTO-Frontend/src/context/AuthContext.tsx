import {createContext} from "react";
import type AuthContextType from "../Interfaces/AuthContext.ts";
import type UgyintezoiJogosultsag from "../Interfaces/UgyintezoiJogosultsag.ts";
import {
    NevKeyName,
    ReszlegKeyName,
    RoleKeyName,
    TokenKeyName,
    UgyintezoiJogosultsagokKeyName
} from "../constants/constants.ts";

export const AuthContext = createContext<AuthContextType>({
    token: localStorage.getItem(TokenKeyName),
    setToken: () => {},
    nev: localStorage.getItem(NevKeyName),
    setNev: () => {},
    reszleg:localStorage.getItem(ReszlegKeyName),
    setReszleg: () => {},
    role: localStorage.getItem(RoleKeyName),
    setRole: () => {},
    ugyintezoiJogosultsagok: JSON.parse(localStorage.getItem(UgyintezoiJogosultsagokKeyName) || 'null') as UgyintezoiJogosultsag[] | null,
    setUgyintezoiJogosultsagok: () => {}
})