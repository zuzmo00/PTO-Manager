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
import type DayOutStatisticsDto from "../Interfaces/DayOutStatisticsDto.ts";
import type statInputDto from "../Interfaces/statInputDto.ts";
import type secondChart from "../Interfaces/secondChart.ts";
import type RequestStatsGetDto from "../Interfaces/RequestStatsGetDto.ts";
import type StatsForRequestInputDto from "../Interfaces/StatsForRequestInputDto.ts";
import type PreferenceDto from "../Interfaces/PreferenceDto.ts";
import type GetPreferenceInputDto from "../Interfaces/GetPreferenceInputDto.ts";
import type GetUsersGetDto from "../Interfaces/GetUsersGetDto.ts";
import type GetUsersInputDto from "../Interfaces/GetUsersInputDto.ts";
import type GetRequestsInputDto from "../Interfaces/GetRequestsInputDto.ts";
import type GetRemainingForUserDto from "../Interfaces/GetRemainingForUserDto.ts";
import type RequestAddAsAdministratorDto from "../Interfaces/RequestAddAsAdministratorDto.ts";
import type PermissionGetDto from "../Interfaces/PermissionGetDto.ts";
import type GetpermissionInputDto from "../Interfaces/GetpermissionInputDto.ts";
import type PermissionUpdateDto from "../Interfaces/permissionUpdateDto.ts";
import type CreateAdminInputDTO from "../Interfaces/CreateAdminInputDTO.ts";
import type RemoveAdminPriviligeInputDto from "../Interfaces/RemoveAdminPriviligeInputDto.ts";
import type SpecialDayRemoveDto from "../Interfaces/SpecialDayRemoveDto.ts";
import type SpecialDaysGetDto from "../Interfaces/SpecialDaysGetDto.ts";
import type SpecialDaysAddDto from "../Interfaces/SpecialDaysAddDto.ts";
import type SpecialDayModifyDto from "../Interfaces/SpecialDayModifyDto.ts";
import type RemoveDepartment from "../Interfaces/RemoveDepartment.ts";
import type CreateDepartmentDto from "../Interfaces/CreateDepartmentDto.ts";
import type DepartmentGetDto from "../Interfaces/DepartmentGetDto.ts";
import type UserRegisterDto from "../Interfaces/UserRegisterDto.ts";

const Auth = {
    login: (dto: loginDto) =>
        axiosInstance.post<ApiResponse<LoginResponse>>("/api/User/Login", dto)
}

const User = {
    postGetUsersByParams: (dto: GetUsersInputDto) =>
        axiosInstance.post<ApiResponse<GetUsersGetDto[]>>("/api/User/GetUsersByParams", dto),
    postGetRemainingDaysByUserid: (dto: GetRemainingForUserDto) =>
        axiosInstance.post<ApiResponse<RemainingDays>>("/api/User/GetRemainingDaysByUserid", dto),
    postRegister: (dto: UserRegisterDto) =>
        axiosInstance.post<ApiResponse<string>>("/api/User/Register", dto),


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
    postGetAcceptedRequests: (dto: PendingRequestsInputDto) =>
        axiosInstance.post<ApiResponse<PendingRequestForAdmins[]>>("/api/Request/GetAcceptedRequestByParams", dto),
    postGetStatsForRequest: (dto: StatsForRequestInputDto) =>
        axiosInstance.post<ApiResponse<RequestStatsGetDto>>("/api/Request/GetStatsForRequest", dto),
    postRevokeRequestAccept: (dto: RevokeRequestInputDto) =>
        axiosInstance.post<ApiResponse<string>>("/api/Request/RevokeRequestAccept", dto),
    postGetAllRequestsAndSpecialDaysByUserId: (dto: GetRequestsInputDto) =>
        axiosInstance.post<ApiResponse<ReservedDays[]>>("/api/Request/GetAllRequestsAndSpecialDaysByUserId", dto),
    postRequestAddAsAdministratorDto: (dto: RequestAddAsAdministratorDto) =>
        axiosInstance.post<ApiResponse<string>>("/api/Request/CreateRequestAsAdministrator", dto),
}


const Department = {
    getDepartments: () =>
        axiosInstance.get<ApiResponse<string[]>>("/api/Department/GetDepartments"),
    getDepartmentsForManage: () =>
        axiosInstance.get<ApiResponse<DepartmentGetDto[]>>("/api/Department/GetDepartmentsForManage"),
    CreateDepartment: (dto: CreateDepartmentDto) =>
        axiosInstance.post<ApiResponse<string>>("/api/Department/CreateDepartment", dto),
    removeRemoveDepartment: (dto: RemoveDepartment) =>
        axiosInstance.delete<ApiResponse<string>>("/api/Department/RemoveDepartment", {data: dto}),


}

const Preferences = {
    postGetPreference: (dto: GetPreferenceInputDto) =>
        axiosInstance.post<ApiResponse<PreferenceDto>>("/api/Preference/GetPreference",dto),

    postCreatePreference: (dto: PreferenceDto) =>
        axiosInstance.post<ApiResponse<string>>("/api/Preference/CreatePreference",dto),

    putModifyPreference: (dto : PreferenceDto ) =>
        axiosInstance.put<ApiResponse<string>>("/api/Preference/ModifyPreference", dto),
}


const Stats = {
    postStatsProDepartment: (dto : statInputDto) =>
        axiosInstance.post<ApiResponse<DayOutStatisticsDto[]>>("/api/Stats/GetStats", dto),

    postStatsForWeek: (dto : statInputDto) =>
        axiosInstance.post<ApiResponse<secondChart[]>>("/api/Stats/GetStatsForWeek", dto)
}

const Admin = {
    postGetPermissionsForUser: (dto :GetpermissionInputDto ) =>
        axiosInstance.post<ApiResponse<PermissionGetDto[]>>("/api/Admin/GetPermissionsForUser", dto),
    putChangePermissions: (dto :PermissionUpdateDto ) =>
        axiosInstance.put<ApiResponse<string>>("/api/Admin/ChangePermissions", dto),
    postCreateAdmin: (dto :CreateAdminInputDTO ) =>
        axiosInstance.post<ApiResponse<string>>("/api/Admin/CreateAdmin", dto),
    deleteRemovePriviligeByParams: (dto :RemoveAdminPriviligeInputDto ) =>
        axiosInstance.delete<ApiResponse<string>>("/api/Admin/RemovePriviligeByParams", {data: dto}),
}


const SpecialDays = {
    postAddSpecialDay: (dto :SpecialDaysAddDto) =>
        axiosInstance.post<ApiResponse<string>>("/api/SpecialDays/AddSpecialDay", dto),

    getListSpecialDays: () =>
        axiosInstance.get<ApiResponse<SpecialDaysGetDto[]>>("/api/SpecialDays/ListSpecialDays"),

    deleteRemoveSpecialDay: (dto : SpecialDayRemoveDto) =>
        axiosInstance.delete<ApiResponse<string>>("/api/SpecialDays/RemoveSpecialDay", {data: dto}),

    modifySpecialDay: (dto : SpecialDayModifyDto) =>
        axiosInstance.put<ApiResponse<string>>("/api/SpecialDays/ModifySpecialDay", dto),


}




const api = {Auth, Request, Department, Stats, Preferences, User, Admin, SpecialDays}

export default api;