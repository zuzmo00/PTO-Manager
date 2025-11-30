import {
    Box, Button,
    Center,
    Container, Divider, Group,
    LoadingOverlay, Modal,
    Paper, Select,
    Table,
    Text,
    Title
} from "@mantine/core";
import {useEffect, useState} from "react";
import {useDisclosure} from "@mantine/hooks";
import { notifications } from "@mantine/notifications";
import api from "../api/api.ts";
import type SpecialDaysGetDto from "../Interfaces/SpecialDaysGetDto.ts";
import {DateInput} from "@mantine/dates";
import dayjs from "dayjs";
import type SpecialDaysAddDto from "../Interfaces/SpecialDaysAddDto.ts";



function ManageSpecialDays () {
    const [isLoading,setIsLoading] = useState(false)
    const [modelOpen, {open, close}] = useDisclosure(false)
    const [deleteModalOpen, {open: openDeleteModal, close: closeDeletModal}] = useDisclosure(false)
    const [modifyModalOpen, {open: openModifyModal, close: closeModifyModal}] = useDisclosure(false)
    const [preferencesModalOpen, {open: openPreferencesModal, close: closePreferencesModal}] = useDisclosure(false)
    const [specialDays, setSpecialDays] = useState<SpecialDaysGetDto[] | []> ([])
    const [dateValue, setDateValue] = useState<string | null>();
    const [currentSpecialDay, setCurrentSpecialDay] = useState<SpecialDaysGetDto | null>();

    const [isWorkDay, setIsWorkDay] = useState<boolean>(false);
    const [modifyModalIsWorkDay, setModifyModalIsWorkDay] = useState<boolean>(false);
    const [isWeekendWorkday, setIsWeekendWorkday] = useState<boolean>(false);

    const fetchData = async () => {
        setIsLoading(true);
        try{
            const specialDaysData = await api.SpecialDays.getListSpecialDays();
            const specialDaysResponseData = specialDaysData.data?.data ?? []
            setSpecialDays(specialDaysResponseData);
            setDateValue(null);

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

    const  HandleNewSpecialDay = async () => {
        setIsLoading(true)
        try{

            const apiInput :SpecialDaysAddDto = {Date: dayjs(dateValue).format('YYYY-MM-DD') , IsWorkingDay: isWorkDay }
            await api.SpecialDays.postAddSpecialDay(apiInput)
            await fetchData();
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            close()
            setIsLoading(false)
        }
    }



    const HandleSpecialDayModify = async (l_bool:boolean) => {
        setIsLoading(true)
        try{

            await api.SpecialDays.modifySpecialDay({Id: currentSpecialDay?.id ?? "", IsWorkingDay:l_bool })
            await fetchData();
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            closeModifyModal()
            close()
            setIsLoading(false)
        }
    }


    const HandleSpecialDayDelete = async (id:string) => {
        setIsLoading(true)
        try{

            await api.SpecialDays.deleteRemoveSpecialDay({dayId: id})
            await fetchData();
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            closeDeletModal()
            close()
            setIsLoading(false)
        }
    }
    const HandlePreferenceSubmit= async () => {
        setIsLoading(true)
        try{

            await api.Preferences.putModifyPreference({name: "hetvege_munkanap_e", value: isWeekendWorkday})
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            closePreferencesModal()
            close()
            setIsLoading(false)
        }
    }

    const HandlePreferenceModal= async () => {
        setIsLoading(true)
        try{

            const response = await api.Preferences.postGetPreference({preferenceName: "hetvege_munkanap_e"});
            setIsWeekendWorkday(response.data?.data?.value ?? true);
            openPreferencesModal();
        }catch {
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
            })
        }finally {
            setIsLoading(false)
        }
    }

    const IsExcludedDate = (date:string) : boolean => {
        const temp_day = dayjs(date);
        const formated_day = temp_day.format("YYYY-MM-DD");
        return specialDays.some(c=> c.date === formated_day);
    };

    return(
        <Container>
            <Paper p="xl" radius="md" withBorder pos = "relative">
                <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
                <Title style={{fontSize:20, textAlign:"center"}}>Kérem válasszon egy különleges napot amelyet módosítani szeretne!</Title>
                <Center>
                    <Box style={{ overflow: "auto" }} mt={20} w="100%">
                        <Table striped highlightOnHover>
                            <Table.Thead>
                                <Table.Tr>
                                    <Table.Th style={{textAlign: "center"}}>Dátum</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}>Munkanap-e</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}></Table.Th>
                                </Table.Tr>
                            </Table.Thead>
                            <Table.Tbody>
                                {specialDays.map(k=>(
                                    <Table.Tr key={k.id}>
                                        <Table.Td style={{textAlign: "center"}}>{k.date}</Table.Td>
                                        <Table.Td  style={{textAlign: "center", color: k.isWorkingDay ? "red" : "green"}}>{k.isWorkingDay ? "Igen" : "Nem"}</Table.Td>
                                        <Table.Td style={{textAlign: "center",display: "flex", justifyContent: "center", alignItems: "center", gap: "10px"}}>
                                            <Button onClick={() => {setModifyModalIsWorkDay(k.isWorkingDay); setCurrentSpecialDay(k); openModifyModal()}}>
                                                Mósodítás
                                            </Button>
                                            <Button color="red" onClick={() => {setCurrentSpecialDay(k); openDeleteModal()}}>
                                                Törlés
                                            </Button>
                                        </Table.Td>
                                    </Table.Tr>
                                ))}
                            </Table.Tbody>
                        </Table>
                    </Box>
                </Center>
                <Group justify={"space-between"} mt={30}>
                    <Button style={{backgroundColor:"green"}} onClick={() => open()}>
                        Különleges nap felvétele
                    </Button>
                    <Button onClick={() => HandlePreferenceModal()} >
                        Preferenciák beállítása
                    </Button>
                </Group>
            </Paper>

            <Modal opened={modelOpen} onClose={close} centered title={"Különleges nap felvétele"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"}>Adja meg a nap paramétereit!</Text>
                        <Divider mt={10} mb={10}/>
                        <Center>
                        <Group justify={"center"}>
                            <DateInput
                                label="Dátum kiválasztása"
                                placeholder="Válassz dátumot"
                                excludeDate={IsExcludedDate}
                                value={dateValue}
                                onChange={setDateValue}
                                w={235}
                            />
                            <Box>
                                <Select
                                    label="Munkanap-e?"
                                    placeholder=""
                                    w={235}
                                    value={isWorkDay ? "Igen" : "Nem"}
                                    data={['Igen', 'Nem']}
                                    onChange={(value) => setIsWorkDay(value === "Igen")}
                                />
                            </Box>
                        </Group>
                        </Center>
                        <Group justify={"center"} mt={20}>
                            <Button style={{backgroundColor:"red"}} onClick={ () => close()}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"green"}} onClick={ () => HandleNewSpecialDay()}>
                                Nap hozzáadása
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>

            <Modal opened={deleteModalOpen} onClose={closeDeletModal} centered title={"Különleges nap törlése"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"} mb={10}>Biztosan törölni szeretné ezt a napot?</Text>
                        <Text><b>Dátum:</b> {currentSpecialDay?.date}</Text>
                        <Text><b>Munkanap-e:</b> {currentSpecialDay?.isWorkingDay? "Igen" : "Nem"}</Text>
                        <Group justify={"center"} mt={10}>
                            <Button onClick={ () => closeDeletModal()}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"red"}} onClick={() => (HandleSpecialDayDelete(currentSpecialDay?.id ?? ""))}>
                                Törlés
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>

            <Modal opened={modifyModalOpen} onClose={closeModifyModal} centered title={"Különleges nap módosítása"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"} mb={10}>Hajtsa végre a szükséges módosításokat!</Text>
                        <Text><b>Dátum:</b> {currentSpecialDay?.date}</Text>
                        <Center>
                            <Box mt={10}>
                                <Select
                                    label="Munkanap-e?"
                                    placeholder=""
                                    w={235}
                                    value={modifyModalIsWorkDay ? "Igen" : "Nem"}
                                    data={['Igen', 'Nem']}
                                    onChange={(value) => setModifyModalIsWorkDay(value === "Igen")}
                                />
                            </Box>
                        </Center>
                        <Group justify={"center"} mt={20}>
                            <Button onClick={ () => closeModifyModal()}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"red"}} onClick={() => HandleSpecialDayModify(modifyModalIsWorkDay)}>
                                Módosítások mentése
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>

            <Modal opened={preferencesModalOpen} onClose={closePreferencesModal} centered title={"Preferencia beállítások"} ta={"center"}>
                <Center>
                    <Box>
                        <Box>
                            <Text fw={"bold"} mb={10}>A hétvége munkanap-e?</Text>
                            <Group justify={"center"} mt={10}>
                                <Select
                                    placeholder=""
                                    w={235}
                                    value={isWeekendWorkday ? "Igen" : "Nem"}
                                    data={['Igen', 'Nem']}
                                    onChange={(value) => setIsWeekendWorkday(value === "Igen")}
                                />
                            </Group>
                        </Box>
                        <Group justify={"center"} mt={20}>
                            <Button onClick={ () => closePreferencesModal()}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"red"}} onClick={() => HandlePreferenceSubmit()}>
                                Módosítások mentése
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>

        </Container>
    );
}

export default ManageSpecialDays;