import {
    Box, Button,
    Center,
    Container, Group,
    LoadingOverlay, Modal,
    Paper, Select,
    Table,
    Text, TextInput,
    Title
} from "@mantine/core";
import {useEffect, useState} from "react";
import { notifications } from "@mantine/notifications";
import api from "../api/api.ts";
import type DepartmentGetDto from "../Interfaces/DepartmentGetDto.ts";
import {useDisclosure} from "@mantine/hooks";




function ManageDepartment () {
    const [isLoading,setIsLoading] = useState(false)
    const [departmentsForManage, setDepartmentsForManage] = useState<DepartmentGetDto[] | []> ([])


    const [deleteModalOpen, {open: openDeleteModal, close: closeDeletModal}] = useDisclosure(false)
    const [currentDepartment, setCurrentDepartment] = useState<DepartmentGetDto | null> (null)


    const [newDepartmentModalOpen, {open: openNewDepartmentModal, close: closeNewDepartmentModal}] = useDisclosure(false)
    const [newDepartmentName, setNewDepartmentName] = useState<string>("");

    const [AddNewWorkerModel, {open: openAddNewWorkerModel, close: closeAddNewWorkerModel}] = useDisclosure(false)



    const [newWorkerEmail, setNewWorkerEmail] = useState("");
    const [newWorkerName, setNewWorkerName] = useState("");
    const [newWorkerEmployeeId, setNewWorkerEmployeeId] = useState("");
    const [newWorkerDepartmentId, setNewWorkerDepartmentId] = useState<string | null>(null);
    const [newWorkerPassword, setNewWorkerPassword] = useState("");
    const [newWorkerAllHoliday, setNewWorkerAllHoliday] = useState("");


    const handleCreateWorker = async () => {
        setIsLoading(true);

        try {
            await api.User.postRegister({
                Email: newWorkerEmail,
                Name: newWorkerName,
                Employeeid: Number(newWorkerEmployeeId),
                DepartmentId: Number(newWorkerDepartmentId),
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
            setNewWorkerDepartmentId(null);
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



    const fetchData = async () => {
        setIsLoading(true);
        try{
            const response = await api.Department.getDepartmentsForManage();
            setDepartmentsForManage(response.data?.data ?? [])


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



    const  HandleDepartmentCreate = async () => {
        setIsLoading(true)
        try{

            await api.Department.CreateDepartment({departmentName: newDepartmentName})
            await fetchData();
            setNewDepartmentName("");
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            closeNewDepartmentModal()
            setIsLoading(false)
        }
    }

    const  HandleDepartmentDelete = async () => {
        setIsLoading(true)
        try{

            await api.Department.removeRemoveDepartment({id: currentDepartment?.id ?? 9999})
            await fetchData();
            setCurrentDepartment(null);
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            closeDeletModal()
            setIsLoading(false)
        }
    }

    useEffect(() => {
        fetchData();
    }, []);



    return(
        <Container>
            <Paper p="xl" radius="md" withBorder pos = "relative">
                <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
                <Title style={{fontSize:20, textAlign:"center"}}>Kérem válasszon egy részleget amelyen módosításokat szeretne végezni!</Title>
                <Center>
                    <Box style={{ overflow: "auto" }} mt={20} w="100%">
                        <Table striped highlightOnHover>
                            <Table.Thead>
                                <Table.Tr>
                                    <Table.Th style={{textAlign: "center"}}>Részlegnév</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}>Ügyintézők száma</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}></Table.Th>
                                </Table.Tr>
                            </Table.Thead>
                            <Table.Tbody>
                                {departmentsForManage.map(k=>(
                                    <Table.Tr key={k.id}>
                                        <Table.Td style={{textAlign: "center"}}>{k.departmentName}</Table.Td>
                                        <Table.Td  style={{textAlign: "center"}}>{k.adminCount}</Table.Td>
                                        <Table.Td style={{textAlign: "center",display: "flex", justifyContent: "center", alignItems: "center", gap: "10px"}}>
                                            <Button color="red" onClick={() => {setCurrentDepartment(k); openDeleteModal()}}>
                                                Részleg törlése
                                            </Button>
                                        </Table.Td>
                                    </Table.Tr>
                                ))}
                            </Table.Tbody>
                        </Table>
                    </Box>
                </Center>
                <Group justify={"space-between"} mt={30}>
                    <Button onClick={() => openNewDepartmentModal()}>
                       Új részleg felvétele
                    </Button>
                    <Button style={{backgroundColor:"green"}} onClick={() => openAddNewWorkerModel()}>
                        Új munkatárs felvétele
                    </Button>
                </Group>
            </Paper>

            <Modal opened={deleteModalOpen} onClose={closeDeletModal} centered title={"Részleg törlése"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"} mb={10}>Biztosan törölni szeretné ezt a részleget?</Text>
                        <Text><b>Részleg:</b> {currentDepartment?.departmentName}</Text>
                        <Group justify={"center"} mt={20}>
                            <Button onClick={ () => closeDeletModal()}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"red"}} onClick={() => (HandleDepartmentDelete())}>
                                Törlés
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>


            <Modal opened={newDepartmentModalOpen} onClose={closeNewDepartmentModal} centered title={"Új részleg felvétele"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"} mb={10}>Adja meg a részleg nevét!</Text>
                        <TextInput
                            mt={10}
                            style={{}}
                            placeholder={"Ide írja a részleg nevét"}
                            value={newDepartmentName}
                            onChange={(e) => setNewDepartmentName(e.currentTarget.value)}
                        />
                        <Group justify={"center"} mt={20}>
                            <Button onClick={ () => closeDeletModal()}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"green"}} onClick={() => (HandleDepartmentCreate())}>
                                Részleg létrehozása
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
                            data={departmentsForManage.map(d => ({
                                value: d.id.toString(),
                                label: d.departmentName
                            }))}
                            value={newWorkerDepartmentId}
                            onChange={setNewWorkerDepartmentId}
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

export default ManageDepartment;