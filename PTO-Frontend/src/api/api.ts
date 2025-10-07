import type loginDto from "../Interfaces/LoginDto";
import axiosInstance from "./axios.config.ts";
import type ApiResponse from "../Interfaces/ApiResponse.ts";
import type {LoginResponse} from "../Interfaces/LoginResponse.ts";

const Auth = {
    login: (dto: loginDto) =>
        axiosInstance.post<ApiResponse<LoginResponse>>("/Bejelentkezes/login", dto)
}



const api = {Auth}

export default api;