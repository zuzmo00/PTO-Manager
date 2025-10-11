import type UgyintezoiJogosultsag from "./UgyintezoiJogosultsag.ts";

export default interface AuthContext{
    token: string | null;
    setToken: (token: string | null) => void;
    nev: string | null;
    setNev: (nev: string | null) =>void;
    email: string | null;
    setEmail: (email: string | null) =>void;
    reszleg:string | null;
    setReszleg: (reszleg: string | null) =>void;
    role: string | null;
    setRole: (role: string) => void;
    ugyintezoiJogosultsagok: UgyintezoiJogosultsag[] | null;
    setUgyintezoiJogosultsagok: (reszlegJogosultsagok: UgyintezoiJogosultsag[] | null) => void;
}
