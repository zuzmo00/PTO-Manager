export default interface CreateAdminInputDTO {
    id:string,
    departmentName:string,
    CanRequest:boolean,
    CanDecide:boolean,
    CanRevoke:boolean
}