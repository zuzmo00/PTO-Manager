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
import type RequestStatsGetDto from "../Interfaces/RequestStatsGetDto.ts";
import type RevokeRequestInputDto from "../Interfaces/RevokeRequestInputDto.ts";
import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";



function AcceptedRequestManage () {
    const [isLoading,setIsLoading] = useState(false)
    const [modelOpen, {open, close}] = useDisclosure(false)
    const [acceptedData, setacceptedData] = useState<PendingRequestForAdmins[] | []> ([])
    const [dropdownValue, setDropdownValue] = useState<string[]|undefined> (undefined)
    const [departmments, setDepartmments] = useState<string[]> ([])
    const [currentRequest, setCurrentRequest] = useState<string> ("")
    const [searchText, setSearchText] = useState<string>("");
    const [currentRequestStats, setCurrentRequestStats] = useState<RequestStatsGetDto | null> ()

    const { ugyintezoiJogosultsagok } = useContext(AuthContext);



    const fetchData = async () => {
        setIsLoading(true);
        try{
            const departmentdata = await api.Department.GetDepartmentsForDecide();
            const l_departments = departmentdata.data?.data ?? []
            setDepartmments(l_departments);

            const inputdata : PendingRequestsInputDto = {departmentIds : l_departments, inputText: ""}
            const Accepted_data_request = await api.Request.postGetAcceptedRequests(inputdata);
            const AcceptedListData = Accepted_data_request.data?.data ?? []

            setacceptedData(AcceptedListData)

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

    const  HandleDeciding = async () => {
        setIsLoading(true)
        try{
            const apiInput :RevokeRequestInputDto = {RequestBlockId: currentRequest}
            await api.Request.postRevokeRequestAccept(apiInput)
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
        if (departmments.length === 0) {
            {/*Race condition*/}
            return;
        }
        const inputdata : PendingRequestsInputDto = {departmentIds : departmments, inputText: searchText}
        const Accepted_data_request = await api.Request.postGetAcceptedRequests(inputdata);
        const AcceptedListData = Accepted_data_request.data?.data ?? []
        setacceptedData(AcceptedListData)
    }


    useEffect(() => {
        setSearchParameters(dropdownValue)
        fetchPendingRequests();
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


    const HasPermissionToRevoke = (_DepartmentName: string) => {
        const privilege = ugyintezoiJogosultsagok?.find(k=>k.departmentName === _DepartmentName)
        return privilege?.canRevoke ?? false;
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
                                    <Table.Th style={{textAlign: "center"}}></Table.Th>
                                </Table.Tr>
                            </Table.Thead>
                            <Table.Tbody>
                                {acceptedData.filter(c=> {
                                    if(serachParameters?.length === 0 || !serachParameters) {
                                        return true;
                                    }
                                    return  serachParameters?.includes(c.department);
                                }).map(k=>(
                                    <Table.Tr key={k.id}>
                                        <Table.Td style={{textAlign: "center"}}>{k.name}</Table.Td>
                                        <Table.Td  style={{textAlign: "center"}}>{k.department}</Table.Td>
                                        <Table.Td  style={{textAlign: "center"}}>{k.begin}</Table.Td>
                                        <Table.Td  style={{textAlign: "center"}}>{k.end}</Table.Td>
                                        <Table.Td  style={{textAlign: "center"}}><Button disabled={!HasPermissionToRevoke(k.department)} style={{backgroundColor:"red"}} onClick={() => {OpenDeciding(k.id); setCurrentRequest(k.id)}}>Visszavonás</Button></Table.Td>
                                    </Table.Tr>
                                ))}
                            </Table.Tbody>
                        </Table>
                    </Box>
                </Center>
            </Paper>

            <Modal opened={modelOpen} onClose={close} centered title={"Kérelem engedély visszavonása"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"}>Biztosan vissza szeretné vonni az engedélyt az alábbi időszakra?</Text>
                        <Divider mt={10} mb={10}/>
                        <Text>Kérvényezett időszak:</Text>
                        <Group justify={"center"}>
                            <Text>{currentRequestStats?.startDate}-től </Text>
                            <Text>{currentRequestStats?.startDate}-ig</Text>
                        </Group>
                        <Text>Az időszak ennyi munkanapot érint: {currentRequestStats?.requiredDayOff}</Text>
                        <Group justify={"center"} mt={10}>
                            <Button style={{backgroundColor:"red"}} onClick={ () => close()}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"green"}} onClick={ () => HandleDeciding()} >
                                Visszavonás
                            </Button>
                        </Group>
                    </Box>
                </Center>
            </Modal>
        </Container>
    );
}

export default AcceptedRequestManage;