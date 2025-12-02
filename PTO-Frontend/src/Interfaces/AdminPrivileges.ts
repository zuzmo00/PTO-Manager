export default interface AdminPrivileges {
    departmentId: number;
    departmentName: string;
    canRequest: boolean;
    canDecide: boolean;
    canRevoke: boolean;
}