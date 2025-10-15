import type loginDto from "../Interfaces/LoginDto";
import axiosInstance from "./axios.config.ts";
import type ApiResponse from "../Interfaces/ApiResponse.ts";
import type {LoginResponse} from "../Interfaces/LoginResponse.ts";
import type ReservedDays from "../Interfaces/ReservedDays.ts";
import type RemainingDays from "../Interfaces/RemainingDays.ts";
import type PendingRequests from "../Interfaces/PendingRequests.ts";
import type RevokeRequestInputDto from "../Interfaces/RevokeRequestInputDto.ts";
import type RequestAddAsUserDto from "../Interfaces/RequestAddAsUserDto.ts";
import type PendingRequestsInputDto from "../Interfaces/PendingRequestsInputDto.ts";
import type PendingRequestForAdmins from "../Interfaces/PendingRequestForAdmins.ts";
import type RequestDecisionInputDto from "../Interfaces/RequestDecisionInputDto.ts";

const Auth = {
    login: (dto: loginDto) =>
        axiosInstance.post<ApiResponse<LoginResponse>>("/api/User/Login", dto)
}
const Request = {
    getReservedDays: () =>
        axiosInstance.get<ApiResponse<ReservedDays[]>>("/api/Request/GetAllRequests"),
    getRemainingDays: () =>
        axiosInstance.get<ApiResponse<RemainingDays>>("/api/User/GetRemainingDays"),
    getPendingRequest: () =>
        axiosInstance.get<ApiResponse<PendingRequests[]>>("/api/Request/getPendingRequest"),
    postRevokeARequest: (dto: RevokeRequestInputDto) =>
        axiosInstance.post<ApiResponse<string>>("/api/Request/RevokeARequest", dto),
    postRevokeRequestBlock: (dto: RevokeRequestInputDto) =>
        axiosInstance.post<ApiResponse<string>>("/api/Request/RevokeWholeRequest", dto),
    postCreateRequest: (dto: RequestAddAsUserDto) =>
        axiosInstance.post<ApiResponse<string>>("/api/Request/CreateRequest", dto),
    postPendingRequestByDepartment: (dto: PendingRequestsInputDto) =>
        axiosInstance.post<ApiResponse<PendingRequestForAdmins[]>>("/api/Request/getPendingRequestByDepartment", dto),
    postMakeDecision: (dto: RequestDecisionInputDto) =>
        axiosInstance.post<ApiResponse<string>>("/api/Request/makeDecision", dto),

}
const Department = {
    getDepartments: () =>
        axiosInstance.get<ApiResponse<string[]>>("/api/Department/GetDepartments")
}



const api = {Auth, Request, Department}

export default api;