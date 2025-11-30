import {
    Box, Button,
    Center,
    Container, Divider, Group,
    LoadingOverlay, Modal,
    MultiSelect,
    Paper, Select,
    Table,
    Text, TextInput,
    Title
} from "@mantine/core";
import {useEffect, useState} from "react";
import {useDisclosure} from "@mantine/hooks";
import { notifications } from "@mantine/notifications";
import api from "../api/api.ts";
import type GetUsersGetDto from "../Interfaces/GetUsersGetDto.ts";
import type GetUsersInputDto from "../Interfaces/GetUsersInputDto.ts";
import {DatePickerInput} from "@mantine/dates";
import dayjs from "dayjs";
import type ReservedDays from "../Interfaces/ReservedDays.ts";
import type RemainingDays from "../Interfaces/RemainingDays.ts";
import type RequestAddAsAdministratorDto from "../Interfaces/RequestAddAsAdministratorDto.ts";
import type PermissionGetDto from "../Interfaces/PermissionGetDto.ts";
import type PermissionUpdateDto from "../Interfaces/permissionUpdateDto.ts";
import type CreateAdminInputDTO from "../Interfaces/CreateAdminInputDTO.ts";
import type RemoveAdminPriviligeInputDto from "../Interfaces/RemoveAdminPriviligeInputDto.ts";



function ManageWorkers () {
    const [isLoading,setIsLoading] = useState(false)
    const [modelOpen, {open, close}] = useDisclosure(false)
    const [permissonModelOpen, {open: openPermission, close: closePermisson}] = useDisclosure(false)
    const [permissonModifyOpen, {open: openPermissionModify, close: closePermissonModify}] = useDisclosure(false)
    const [addNewPermissonModifyOpen, {open: openAddNewPermissonModifyOpen, close: closeAddNewPermissonModifyOpen}] = useDisclosure(false)

    const [deleteModalOpen, {open: openDeleteModal, close: closeDeletModal}] = useDisclosure(false)
    const [currentPriviligeDepartmentToDelete, setCurrentPriviligeDepartmentToDelete] = useState<string>("");

    const [dropdownValue, setDropdownValue] = useState<string[]|undefined> (undefined)
    const [departmments, setDepartmments] = useState<string[]> ([])
    const [searchText, setSearchText] = useState<string>("");
    const [currentUser, setcurrentUser] = useState<string> ("")
    const [CurrentUserStat, setCurrentUserStat] = useState<RemainingDays | null> ()

    const [userData, setUserData] = useState<GetUsersGetDto[] | []> ([])

    const [weekendWorkday, setWeekendWorkday] = useState<boolean>(false);
    const [value, setValue] = useState<[string | null, string | null]>([null, null]);
    const today = dayjs().add(1,"day").startOf("day").toDate();
    const [reservedDays, setReservedDays] = useState<ReservedDays[]> ([]);

    const [userPermissons, setUserPermissons] = useState<PermissionGetDto[]> ([]);
    const [currentPermissonModify, setCurrentPermissonModify] = useState<PermissionGetDto | null> ()

    const RequestTypes = [{name:"PTO",value:1},{name:"Betegszabadság",value:2}, {name:"Üzleti út",value:3}, {name:"Betervezett szabadság",value:4}]
    const [selectedRequestTypes, setSelectedRequestTypes] = useState<string | null> ("")

    const [canRequestValue, setCanRequestValue] = useState<boolean>();
    const [canDecideValue, setCanDecideValue] = useState<boolean>();
    const [canRevokeValue, setCanRevokeValue] = useState<boolean>();

    const [newAddCanRequestValue, setNewAddCanRequestValue] = useState<boolean>();
    const [newAddCanDecideValue, setNewAddCanDecideValue] = useState<boolean>();
    const [newAddCanRevokeValue, setNewAddCanRevokeValue] = useState<boolean>();
    const [newAddDepartmentValue, setNewAddDepartmentValue] = useState<string>("");

    const [AddNewWorkerModel, {open: openAddNewWorkerModel, close: closeAddNewWorkerModel}] = useDisclosure(false)

    const fetchData = async () => {
        setIsLoading(true);
        try{
            const departmentdata = await api.Department.getDepartments();
            setDepartmments(departmentdata.data?.data ?? []);

            const inputdata : GetUsersInputDto = {departmentIds : departmentdata.data?.data ?? [], inputText: searchText}
            const userDataResponse = await api.User.postGetUsersByParams(inputdata)
            setUserData(userDataResponse.data?.data ?? [])

        }
        catch(error){
            console.log(error);
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérés során!",
                color:"red"
            })
        }
        finally {
            setIsLoading(false)
        }
    }

    useEffect(() => {
        fetchData();

    }, []);

    const  HandleRequestSend = async () => {
        setIsLoading(true)
        try{

            const typenumber = RequestTypes.find(c=> c.name === selectedRequestTypes)?.value
            const apiInput :RequestAddAsAdministratorDto = {UserId: currentUser, Begin_date: value[0] ?? "", End_date: value[1] ?? "", RequestType: typenumber!}
            await api.Request.postRequestAddAsAdministratorDto(apiInput);
            setValue([null, null]);
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            await fetchPendingRequests();
            close()
            setIsLoading(false)

        }
    }


    const HandlePermissionModify = async () => {
        setIsLoading(true)
        try{

            const apiInput :PermissionUpdateDto = {Userid: currentUser, DepartmentId: Number(currentPermissonModify?.departmentId), CanDecide: canDecideValue ?? false, CanRequest: canRequestValue ?? false, CanRevoke: canRevokeValue ?? false}
            await api.Admin.putChangePermissions(apiInput)
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            closePermissonModify();
            closePermisson();
            setIsLoading(false)

        }
    }


    const HandlePermissionDelete = async () => {
        setIsLoading(true)
        try{

            const apiInput :RemoveAdminPriviligeInputDto = { departmentName: currentPriviligeDepartmentToDelete, id: currentUser}
            await api.Admin.deleteRemovePriviligeByParams(apiInput)
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            closeDeletModal();
            closePermisson();
            setIsLoading(false)
        }
    }



    const HandleNewPermissionAdd = async () => {
        setIsLoading(true)
        try{

            const apiInput :CreateAdminInputDTO = {id: currentUser, departmentName: newAddDepartmentValue, CanDecide: newAddCanDecideValue ?? false, CanRequest: newAddCanRequestValue ?? false, CanRevoke: newAddCanRevokeValue ?? false}
            await api.Admin.postCreateAdmin(apiInput)
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            closeAddNewPermissonModifyOpen();
            closePermisson();
            setIsLoading(false)
        }
    }

    const [serachParameters, setSearchParameters] = useState<string[] | undefined> (undefined)

    const fetchPendingRequests = async () =>{
        const inputdata : GetUsersInputDto = {departmentIds : departmments, inputText: searchText}
        const userDataResponse = await api.User.postGetUsersByParams(inputdata)
        setUserData(userDataResponse.data?.data ?? [])
    }

    useEffect(() => {
        setSearchParameters(dropdownValue)
    }, [dropdownValue]);


    const IsExcludedDate = (date:string) : boolean => {
        const temp_day = dayjs(date);
        const formated_day = temp_day.format("YYYY-MM-DD");

        let isWeekend:boolean = false

        const excludedDate = reservedDays.some(c=> c.reservedDay === formated_day);
        if(!weekendWorkday){
            isWeekend = temp_day.day() === 6 || temp_day.day() === 0;
        }

        return isWeekend || excludedDate;
    };



    const OpenRequestModal = async (id: string) => {

        setIsLoading(true);
        try{

            const ReservedDays_response = await api.Request.postGetAllRequestsAndSpecialDaysByUserId({userId: id});
            setReservedDays(ReservedDays_response.data?.data ?? [])

            const RemainingDays_response = await api.User.postGetRemainingDaysByUserid({userId: id});
            setCurrentUserStat(RemainingDays_response.data?.data ?? null)


            const isWeekdayOff = await api.Preferences.postGetPreference({preferenceName: "hetvege_munkanap_e"});
            setWeekendWorkday(isWeekdayOff.data?.data?.value ?? false)

            open();
        }catch (e){
            console.log(e);
        }finally {
            setIsLoading(false);
        }
    }

    const OpenPermissonModal = async (id: string) => {

        setIsLoading(true);
        try{

          //dolgozo permissonjei
            const permissonResponse = await api.Admin.postGetPermissionsForUser({userId: id});
            const permissonData = permissonResponse.data?.data ?? [];
            setUserPermissons(permissonData)

            openPermission();
        }catch (e){
            console.log(e);
        }finally {
            setIsLoading(false);
        }

    }


    const [newWorkerEmail, setNewWorkerEmail] = useState("");
    const [newWorkerName, setNewWorkerName] = useState("");
    const [newWorkerEmployeeId, setNewWorkerEmployeeId] = useState("");
    const [newWorkerDepartment, setnewWorkerDepartment] = useState<string | null>(null);
    const [newWorkerPassword, setNewWorkerPassword] = useState("");
    const [newWorkerAllHoliday, setNewWorkerAllHoliday] = useState("");


    const handleCreateWorker = async () => {
        setIsLoading(true);

        try {
            await api.User.postRegister({
                Email: newWorkerEmail,
                Name: newWorkerName,
                Employeeid: Number(newWorkerEmployeeId),
                DepartmentName: newWorkerDepartment!,
                Password: newWorkerPassword,
                AllHoliday: Number(newWorkerAllHoliday)
            });

            notifications.show({
                title: "Siker",
                message: "Munkatárs sikeresen felvéve!",
                color: "green"
            });

            setNewWorkerEmail("");
            setNewWorkerName("");
            setNewWorkerEmployeeId("");
            setnewWorkerDepartment(null);
            setNewWorkerPassword("");
            setNewWorkerAllHoliday("");

            closeAddNewWorkerModel();

            await fetchData();
        }
        catch(error){
            console.log(error);
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérés során!",
                color:"red"
            })
        }
        finally {
            setIsLoading(false);
        }
    };

    const OpenPermissonModifyModal = (permission: PermissionGetDto) => {
        setCurrentPermissonModify(permission);
        setCanRequestValue(permission.canRequest);
        setCanDecideValue(permission.canDecide);
        setCanRevokeValue(permission.canRevoke);
        openPermissionModify();
    };



    return(
        <Container size="lg">
            <Paper p="xl" radius="md" withBorder pos = "relative">
                <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
                <Title style={{fontSize:20, textAlign:"center"}}>Kérem válasszon mely részlegek dolgozóit szeretné látni!</Title>
                <Group justify={"space-between"}>
                    <MultiSelect
                        label="Részlegek választása"
                        placeholder={"pl.: IT"}
                        data = {departmments}
                        value={dropdownValue}
                        onChange={setDropdownValue}
                        searchable
                        mt={10}

                    />
                    <Group>
                        <TextInput
                            mt={35}
                            style={{}}
                            placeholder={"Írjon be egy nevet"}
                            value={searchText}
                            onChange={(e) => setSearchText(e.currentTarget.value)}
                            onKeyDown={(e) => e.key === "Enter" && fetchPendingRequests()}
                        />
                        <Button mt={35} onClick={fetchPendingRequests}>Keresés</Button>
                    </Group>
                </Group>
                <Center>
                    <Box style={{ overflow: "auto" }} mt={20} w="100%">
                        <Table striped highlightOnHover>
                            <Table.Thead>
                                <Table.Tr>
                                    <Table.Th style={{textAlign: "center"}}>Név</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}>Részleg</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}>Törzsszám</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}>Jogkör</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}>E-mail</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}></Table.Th>
                                </Table.Tr>
                            </Table.Thead>
                            <Table.Tbody>
                                {userData.filter(c=> {
                                    if(serachParameters?.length === 0 || !serachParameters) {
                                        return true;
                                    }
                                    return  serachParameters?.includes(c.departmentName);
                                }).map(k=>(
                                    <Table.Tr key={k.id}>
                                        <Table.Td style={{textAlign: "center"}}>{k.name}</Table.Td>
                                        <Table.Td style={{textAlign: "center"}}>{k.departmentName}</Table.Td>
                                        <Table.Td style={{textAlign: "center"}}>{k.employeeId}</Table.Td>
                                        <Table.Td style={{textAlign: "center"}}>{k.role}</Table.Td>
                                        <Table.Td style={{textAlign: "center"}}>{k.email}</Table.Td>
                                        <Table.Td style={{textAlign: "center",display: "flex", justifyContent: "center", alignItems: "center", gap: "10px"}}>
                                              <Button onClick={() => {OpenRequestModal(k.id); setcurrentUser(k.id)}}>
                                                    Szabadság felvétel
                                                </Button>
                                                <Button color="red" onClick={() => {OpenPermissonModal(k.id); setcurrentUser(k.id)}}>
                                                    Jogkör módosítás
                                                </Button>
                                        </Table.Td>
                                    </Table.Tr>
                                ))}
                            </Table.Tbody>
                        </Table>
                    </Box>
                </Center>
                <Group mt={30}>
                    <Button style={{backgroundColor:"green"}} onClick={() => openAddNewWorkerModel()}>
                        Új munkatárs felvétele
                    </Button>
                </Group>
            </Paper>

            <Modal opened={modelOpen} onClose={close} centered title={"Kérelem leadása"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"}>A dolgozó szabadság adatai:</Text>
                        <Box>
                            <Text mt={10}>Összes kivehető szabasága: {CurrentUserStat?.allHoliday}</Text>
                            <Text>Meglévő szabadságok száma: {CurrentUserStat?.remainingDays}</Text>
                            <Text>Időarányosan kivehető napok száma: {CurrentUserStat?.timeProportional}</Text>
                        </Box>
                        <Divider mt={10} mb={10}/>
                        <DatePickerInput
                            type="range"
                            allowSingleDateInRange
                            label="Időszak kiválasztása"
                            placeholder="Például: Október 20, 2025 - Október 24, 2025"
                            style={{minWidth: 200}}
                            value={value}
                            minDate={today}
                            onChange={setValue}
                            excludeDate={IsExcludedDate}
                            locale="hu"
                        />
                        <Box mt={10}>
                            <Select
                                label="Szabadság típusa"
                                placeholder=""
                                value={selectedRequestTypes}
                                data={RequestTypes.map(c=>c.name)}
                                onChange={setSelectedRequestTypes}
                            />
                        </Box>
                        <Group justify={"center"} mt={10}>
                            <Button style={{backgroundColor:"red"}} onClick={ () => close()}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"green"}}  disabled={value == null} onClick={ () => HandleRequestSend()}>
                                Leadás
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>

            <Modal size={"lg"} opened={permissonModelOpen} onClose={closePermisson} centered title={"Jogkör módosítás"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"} mb={20}>A dolgozó jogosultságai:</Text>
                        <Center>
                            <Box style={{ overflow: "auto" }} w="100%">
                                <Table striped highlightOnHover>
                                    <Table.Thead>
                                        <Table.Tr>
                                            <Table.Th style={{textAlign: "center"}}>Részleg</Table.Th>
                                            <Table.Th style={{textAlign: "center"}}>Kérhet</Table.Th>
                                            <Table.Th style={{textAlign: "center"}}>Dönthet</Table.Th>
                                            <Table.Th style={{textAlign: "center"}}>Visszavonhat</Table.Th>
                                            <Table.Th style={{textAlign: "center"}}></Table.Th>
                                        </Table.Tr>
                                    </Table.Thead>
                                    <Table.Tbody>
                                        {userPermissons.map(k=>(
                                            <Table.Tr key={k.departmentId}>
                                                <Table.Td style={{textAlign: "center"}}>{k.departmentName}</Table.Td>
                                                <Table.Td style={{textAlign: "center", fontWeight: "bold" ,color: k.canRequest ? "green" : "red"}}>{k.canRequest ? "Igen" : "Nem"}</Table.Td>
                                                <Table.Td style={{textAlign: "center", fontWeight: "bold" ,color: k.canDecide ? "green" : "red"}}>{k.canDecide ? "Igen" : "Nem"}</Table.Td>
                                                <Table.Td style={{textAlign: "center", fontWeight: "bold" ,color: k.canRevoke ? "green" : "red"}}>{k.canRevoke ? "Igen" : "Nem"}</Table.Td>
                                                <Table.Td style={{textAlign: "center",display: "flex", justifyContent: "center", alignItems: "center", gap: "10px"}}>
                                                    <Button onClick={() => {OpenPermissonModifyModal(k);}}>
                                                        Mósodítás
                                                    </Button>
                                                    <Button color="red" onClick={() => {openDeleteModal(); setCurrentPriviligeDepartmentToDelete(k.departmentName)}}>
                                                        Törlés
                                                    </Button>
                                                </Table.Td>
                                            </Table.Tr>
                                        ))}
                                    </Table.Tbody>
                                </Table>
                            </Box>
                        </Center>
                        <Group justify={"center"} mt={50}>
                            <Button style={{backgroundColor:"red"}} onClick={ () => closePermisson()}>
                                Mégsem
                            </Button>
                            <Button onClick={() => openAddNewPermissonModifyOpen()}>
                                Új jogkör hozzáadása
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>


            <Modal opened={permissonModifyOpen} onClose={closePermissonModify} centered title={"Jogkör módosítása"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"}>{currentPermissonModify?.departmentName} jogkör módosítása</Text>
                        <Box>
                            <Box mt={10}>
                                <Text>Kérhet-e?</Text>
                                <Select
                                    placeholder=""
                                    value={canRequestValue ? "Igen" : "Nem"}
                                    data={['Igen', 'Nem']}
                                    onChange={(value) => setCanRequestValue(value === "Igen")}
                                />
                            </Box>
                            <Box mt={10}>
                                <Text>Engedélyezhet-e?</Text>
                                <Select
                                    placeholder=""
                                    value={canDecideValue ? "Igen" : "Nem"}
                                    data={['Igen', 'Nem']}
                                    onChange={(value) => setCanDecideValue(value === "Igen")}
                                />
                            </Box>
                            <Box mt={10}>
                                <Text>Visszavonhat-e?</Text>
                                <Select
                                    placeholder=""
                                    value={canRevokeValue ? "Igen" : "Nem"}
                                    data={['Igen', 'Nem']}
                                    onChange={(value) => setCanRevokeValue(value === "Igen")}
                                />
                            </Box>

                        </Box>
                        <Group justify={"center"} mt={20}>
                            <Button style={{backgroundColor:"red"}} onClick={ () => closePermissonModify()}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"green"}}  disabled={value == null} onClick={ () => HandlePermissionModify()}>
                                Változtatások mentése
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>
            <Modal opened={addNewPermissonModifyOpen} onClose={closeAddNewPermissonModifyOpen} centered title={"Új jogkör hozzáadása"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"}>{currentPermissonModify?.departmentName} Új jogkör hozzáadása</Text>
                        <Box>
                            <Box mt={10}>
                                <Text>Melyik részlegre?</Text>
                                <Select
                                    placeholder="Válasszon részleget!"
                                    data={departmments.filter(c=> !userPermissons.some(p => p.departmentName === c))}
                                    onChange={(value) => setNewAddDepartmentValue(value ?? "")}
                                />
                            </Box>
                            <Box mt={10}>
                                <Text>Kérhet-e?</Text>
                                <Select
                                    placeholder=""
                                    defaultValue={"Nem"}
                                    data={['Igen', 'Nem']}
                                    onChange={(value) => setNewAddCanRequestValue(value === "Igen")}
                                />
                            </Box>
                            <Box mt={10}>
                                <Text>Engedélyezhet-e?</Text>
                                <Select
                                    placeholder=""
                                    defaultValue={"Nem"}
                                    data={['Igen', 'Nem']}
                                    onChange={(value) => setNewAddCanDecideValue(value === "Igen")}
                                />
                            </Box>
                            <Box mt={10}>
                                <Text>Visszavonhat-e?</Text>
                                <Select
                                    placeholder=""
                                    defaultValue={"Nem"}
                                    data={['Igen', 'Nem']}
                                    onChange={(value) => setNewAddCanRevokeValue(value === "Igen")}
                                />
                            </Box>
                        </Box>
                        <Group justify={"center"} mt={20}>
                            <Button style={{backgroundColor:"red"}} onClick={ () => closeAddNewPermissonModifyOpen()}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"green"}} onClick={ () => HandleNewPermissionAdd()}>
                                Jogosultság hozzáadása
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>

            <Modal opened={deleteModalOpen} onClose={closeDeletModal} centered title={"Jogkör törlése"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"} mb={20}>Biztosan törölni szeretné a jogosultságot?</Text>
                        <Group justify={"center"} mt={10}>
                            <Button style={{backgroundColor:"red"}} onClick={ () => closeDeletModal()}>
                                Mégsem
                            </Button>
                            <Button onClick={() => HandlePermissionDelete()}>
                                Törlés
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>

            <Modal opened={AddNewWorkerModel} onClose={closeAddNewWorkerModel} centered title={"Új munkatárs felvétele"} ta={"center"}>
                <Center>
                    <Box w={350}>
                        <Text fw={"bold"} mb={10}>Adja meg az új munkatárs adatait!</Text>

                        <TextInput
                            label="Email"
                            placeholder="Email"
                            value={newWorkerEmail}
                            onChange={(e) => setNewWorkerEmail(e.currentTarget.value)}
                            mt={15}
                            required
                        />

                        <TextInput
                            label="Név"
                            placeholder="Munkatárs neve"
                            value={newWorkerName}
                            onChange={(e) => setNewWorkerName(e.currentTarget.value)}
                            mt={15}
                            required
                        />

                        <TextInput
                            label="Törzsszám"
                            placeholder="Pl.: 123"
                            value={newWorkerEmployeeId}
                            onChange={(e) => setNewWorkerEmployeeId(e.currentTarget.value)}
                            mt={15}
                            required
                        />

                        <Select
                            label="Részleg"
                            placeholder="Válasszon részleget"
                            data={departmments.map(d => ({
                                value: d,
                                label: d
                            }))}
                            value={newWorkerDepartment}
                            onChange={setnewWorkerDepartment}
                            mt={15}
                            required
                        />

                        <TextInput
                            label="Jelszó"
                            placeholder="Jelszó"
                            type="password"
                            value={newWorkerPassword}
                            onChange={(e) => setNewWorkerPassword(e.currentTarget.value)}
                            mt={15}
                            required
                        />

                        <TextInput
                            label="Éves szabadságkeret"
                            placeholder="Pl.: 26"
                            value={newWorkerAllHoliday}
                            onChange={(e) => setNewWorkerAllHoliday(e.currentTarget.value)}
                            mt={15}
                            required
                        />

                        <Group justify="center" mt={20}>
                            <Button onClick={closeAddNewWorkerModel}>
                                Mégsem
                            </Button>

                            <Button style={{ backgroundColor: "green" }} onClick={handleCreateWorker}>
                                Mentés
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>
        </Container>
    );
}

export default ManageWorkers;