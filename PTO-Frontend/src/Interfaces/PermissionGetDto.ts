export default interface PermissionGetDto {
    departmentId: string,
    departmentName: string,
    canRequest: boolean,
    canDecide: boolean,
    canRevoke: boolean
}