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



function AcceptedRequestManage () {
    const [isLoading,setIsLoading] = useState(false)
    const [modelOpen, {open, close}] = useDisclosure(false)
    const [pendingData, setPendingData] = useState<PendingRequestForAdmins[] | []> ([])
    const [dropdownValue, setDropdownValue] = useState<string[]|undefined> (undefined)
    const [departmments, setDepartmments] = useState<string[]> ([])

    const [currentRequest, setCurrentRequest] = useState<string> ("")

    const templateList:PendingRequestForAdmins[] = [{
        id:"34534534hbe",
        name:"string",
        department:"IT",
        begin: "string",
        end:"string",
        requestId:"string"
    },
        {
        id:"3453443534hbe",
        name:"string",
        department:"HR",
        begin: "string",
        end:"string",
        requestId:"string"
    },
        {
            id:"345347569534hbewsess",
            name:"string",
            department:"Pénzügy",
            begin: "string",
            end:"string",
            requestId:"string"
        },
        {
            id:"345323424534hhhhhhbe",
            name:"string",
            department:"IT",
            begin: "string",
            end:"string",
            requestId:"string"
        },
    ]

    const fetchData = async () => {
        setIsLoading(true);
        try{
            const departmentdata = await api.Department.getDepartments();
            const l_departments = departmentdata.data?.data ?? []
            setDepartmments(l_departments);

            const requestsByDepartment_data = templateList.filter(c=> l_departments.includes(c.department))
            setPendingData(requestsByDepartment_data)

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
            fetchPendingRequests();
            close()
            setIsLoading(false)

        }
    }

    const [serachParameters, setSearchParameters] = useState<string[] | undefined> (undefined)

    const fetchPendingRequests = async () =>{
        const inputdata : PendingRequestsInputDto = {departmentIds : departmments}

        const requestsByDepartment_data = templateList.filter(c=> departmments.includes(c.department))

        setPendingData(requestsByDepartment_data)
    }

    {
        useEffect(() => {
            setSearchParameters(dropdownValue)
            fetchPendingRequests();
        }, [dropdownValue]);
    }

    const OpenDeciding = () => {
        open();
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
                        />
                        <Button mt={35}>Keresés</Button>
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
                                {pendingData.filter(c=> {
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
                                        <Table.Td  style={{textAlign: "center"}}><Button style={{backgroundColor:"red"}} onClick={() => {OpenDeciding(); setCurrentRequest(k.id)}}>Visszavonás</Button></Table.Td>
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
                            <Text>2025-10-27-től </Text>
                            <Text>2025-10-29-ig</Text>
                        </Group>
                        <Text>Az időszak ennyi munkanapot érint: 3</Text>
                        <Group justify={"center"} mt={10}>
                            <Button style={{backgroundColor:"red"}}>
                                Mégsem
                            </Button>
                            <Button style={{backgroundColor:"green"}} >
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