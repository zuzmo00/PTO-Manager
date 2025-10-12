import type loginDto from "../Interfaces/LoginDto";
import axiosInstance from "./axios.config.ts";
import type ApiResponse from "../Interfaces/ApiResponse.ts";
import type {LoginResponse} from "../Interfaces/LoginResponse.ts";
import type ReservedDays from "../Interfaces/ReservedDays.ts";
import type RemainingDays from "../Interfaces/RemainingDays.ts";

const Auth = {
    login: (dto: loginDto) =>
        axiosInstance.post<ApiResponse<LoginResponse>>("/api/User/Login", dto)
}
const Request = {
    getReservedDays: () =>
        axiosInstance.get<ApiResponse<ReservedDays[]>>("/api/Requiest/GetAllRequests"),
    getRemainingDays: () =>
        axiosInstance.get<ApiResponse<RemainingDays>>("/api/User/GetRemainingDays"),




}



const api = {Auth, Request}

export default api;