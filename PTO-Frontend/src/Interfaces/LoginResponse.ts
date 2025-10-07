import type UgyintezoiJogosultsag from "./UgyintezoiJogosultsag.ts";

export type LoginResponse = {
    token: string
    ugyintezoiJogosultsagok: UgyintezoiJogosultsag[]
};