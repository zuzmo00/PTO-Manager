import {
    Box, Button,
    Center,
    Container, Divider, Group,
    LoadingOverlay, Modal,
    MultiSelect,
    Paper,
    Table,
    Text, TextInput,
    Title
} from "@mantine/core";
import {useEffect, useState} from "react";
import {useDisclosure} from "@mantine/hooks";
import type PendingRequestForAdmins from "../Interfaces/PendingRequestForAdmins.ts";
import { notifications } from "@mantine/notifications";
import api from "../api/api.ts";
import type PendingRequestsInputDto from "../Interfaces/PendingRequestsInputDto.ts";
import type RequestDecisionInputDto from "../Interfaces/RequestDecisionInputDto.ts";
import type RequestStatsGetDto from "../Interfaces/RequestStatsGetDto.ts";



function Deciding () {
    const [isLoading,setIsLoading] = useState(false)
    const [modelOpen, {open, close}] = useDisclosure(false)
    const [pendingData, setPendingData] = useState<PendingRequestForAdmins[] | []> ([])
    const [dropdownValue, setDropdownValue] = useState<string[]|undefined> (undefined)
    const [departmments, setDepartmments] = useState<string[]> ([])
    const [searchText, setSearchText] = useState<string>("");
    const [currentRequest, setCurrentRequest] = useState<string> ("")
    const [currentRequestStats, setCurrentRequestStats] = useState<RequestStatsGetDto | null> ()

    const fetchData = async () => {
        setIsLoading(true);
        try{
            const departmentdata = await api.Department.getDepartments();
            setDepartmments(departmentdata.data?.data ?? []);

            const inputdata : PendingRequestsInputDto = {departmentIds : departmentdata.data?.data ?? [], inputText: ""}
            const requestsByDepartment_data = await api.Request.postPendingRequestByDepartment(inputdata);

            setPendingData(requestsByDepartment_data.data?.data ?? [])


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

    const  HandleDeciding = async (BoolDecide:boolean) => {
        setIsLoading(true)
        try{
            const apiInput :RequestDecisionInputDto = {RequestBlockId: currentRequest, verdict: BoolDecide}
            await api.Request.postMakeDecision(apiInput);
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

    const [serachParameters, setSearchParameters] = useState<string[] | undefined> (undefined)

    const fetchPendingRequests = async () =>{
        const inputdata : PendingRequestsInputDto = {departmentIds : departmments , inputText: searchText}
        const requestsByDepartment_data = await api.Request.postPendingRequestByDepartment(inputdata);

        setPendingData(requestsByDepartment_data.data?.data ?? [])
    }

    useEffect(() => {
        setSearchParameters(dropdownValue)
    }, [dropdownValue]);


    const OpenDeciding = async (id: string) => {
        setIsLoading(true);
        try{
            const response = await api.Request.postGetStatsForRequest({requestBlockId: id});
            const response_data = response.data?.data;
            setCurrentRequestStats(response_data)
            open();
        }catch (e){
            console.log(e);
        }finally {
            setIsLoading(false);
        }

    }

    return(
        <Container>
            <Paper p="xl" radius="md" withBorder pos = "relative">
                <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
                    <Title style={{fontSize:20, textAlign:"center"}}>Kérem válasszon mely részlegek kérelmeit szeretné látni!</Title>
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
                                    <Table.Th style={{textAlign: "center"}}>Tól</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}>Ig</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}>Kérelem azonosító</Table.Th>
                                    <Table.Th style={{textAlign: "center"}}></Table.Th>
                                </Table.Tr>
                            </Table.Thead>
                            <Table.Tbody>
                                {pendingData.filter(c=> {
                                    if(serachParameters?.length === 0 || !serachParameters) {
                                        return true;
                                    }
                                    return  serachParameters?.includes(c.department);
                                }).map(k=>(
                                    <Table.Tr key={k.id}>
                                        <Table.Td>{k.name}</Table.Td>
                                        <Table.Td>{k.department}</Table.Td>
                                        <Table.Td>{k.begin}</Table.Td>
                                        <Table.Td>{k.end}</Table.Td>
                                        <Table.Td>{k.id}</Table.Td>
                                        <Table.Td><Button style={{backgroundColor:"red"}} onClick={() => {setCurrentRequest(k.id) ; OpenDeciding(k.id)}}>Bírálat</Button></Table.Td>
                                    </Table.Tr>
                                ))}
                            </Table.Tbody>
                        </Table>
                    </Box>
                </Center>
            </Paper>

            <Modal opened={modelOpen} onClose={close} centered title={"Kérelem Bírálat"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"}>A kérelmező szabadság adatai:</Text>
                        <Box>
                            <Text mt={10}>Összes kivehető szabasága: {currentRequestStats?.allHoliday}</Text>
                            <Text>Meglévő szabadságok száma: {currentRequestStats?.remainingDays}</Text>
                            <Text>Időarányosan kivehető napok száma: {currentRequestStats?.timeProportional}</Text>
                        </Box>
                        <Divider mt={10} mb={10}/>
                        <Text>Kérvényezett időszak:</Text>
                        <Group justify={"center"}>
                            <Text>{currentRequestStats?.startDate}-től </Text>
                            <Text>{currentRequestStats?.endDate}-ig</Text>
                        </Group>
                        <Text>Az időszak ennyi munkanapot érint: {currentRequestStats?.requiredDayOff}</Text>
                        <Group justify={"center"} mt={10}>
                            <Button style={{backgroundColor:"red"}} onClick={ () => HandleDeciding(false)}>
                                Elutasítás
                            </Button>
                            <Button style={{backgroundColor:"green"}} onClick={ () => HandleDeciding(true)}>
                                Elfogadás
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>
        </Container>
    );
}

export default Deciding;