export default interface PermissionUpdateDto {
    Userid:string,
    DepartmentId:number,
    CanRequest:boolean,
    CanDecide:boolean,
    CanRevoke:boolean
}
