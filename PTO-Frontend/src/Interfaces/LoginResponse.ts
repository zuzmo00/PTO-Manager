import type AdminPrivileges from "./AdminPrivileges.ts";

export type LoginResponse = {
    token: string
    adminPrivileges: AdminPrivileges[]
};