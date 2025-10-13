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


function Deciding () {
    const [isLoading,setIsLoading] = useState(false)
    const [modaloOpen, {open, close}] = useDisclosure(false)
    const [pendingData, setPendingData] = useState<PendingRequestForAdmins[] | []> ([])
    const [dropdownValue, setDropdownValue] = useState<string[]|undefined> (undefined)



    const fetchData = async () => {
        setIsLoading(true);
        try{
            //await api.Request


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
        setPendingData([
            {
                id: "elso",
                name:"nagy andras",
                department: "IT",
                begin: "2025-10-21",
                end: "2025-10-23",
                requestId: "sdfh34-436534gv-423vgt",
            },
            {
                id: "masodik",
                name:"kiss mariann",
                department: "HR",
                begin: "2025-10-14",
                end: "2025-10-16",
                requestId: "sdfh34-dfgdgfdfg-423vgt",
            },
            {
                id: "harmadik",
                name:"Gaspar tamas",
                department: "Gyartas",
                begin: "2025-10-12",
                end: "2025-10-14",
                requestId: "sdfgg-436534gv-423vgt",
            },
            {
                id: "negyedik",
                name:"penz ugy",
                department: "Karbantartas",
                begin: "2025-11-09",
                end: "2025-11-14",
                requestId: "sdfgg-436534gv-423vgt",
            },

        ]);
    }, []);

    const HandleDeciding = () => {

    }

    const [serachParameters, setSearchParameters] = useState<string[] | undefined> (undefined)

    useEffect(() => {
        setSearchParameters(dropdownValue)
        fetchPendingRequests();
    }, [dropdownValue]);
    const fetchPendingRequests = () =>{
        //await lekeres
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
                            data = {["IT","HR","Gyartas","Karbantartas"]}
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
                                        <Table.Td>{k.requestId}</Table.Td>
                                        <Table.Td><Button style={{backgroundColor:"red"}} onClick={OpenDeciding}>Bírálat</Button></Table.Td>
                                    </Table.Tr>
                                ))}
                            </Table.Tbody>
                        </Table>
                    </Box>
                </Center>
            </Paper>

            <Modal opened={modaloOpen} onClose={close} centered title={"Kérelem Bírálat"} ta={"center"}>
                <Center>
                    <Box>
                        <Text fw={"bold"}>A kérelmező szabadság adatai:</Text>
                        <Box>
                            <Text mt={10}>Összes kivehető szabasága: 30</Text>
                            <Text>Eddig kivett szabadságnapok száma: 10</Text>
                            <Text>Időarányosan kivehető napok száma: 14</Text>
                        </Box>
                        <Divider mt={10} mb={10}/>
                        <Text>Kérvényezett időszak:</Text>
                        <Group justify={"center"}>
                            <Text>2025-10-21-től </Text>
                            <Text>2025-10-23-ig</Text>
                        </Group>
                        <Text>Az időszak ennyi munkanapot érint: 3</Text>
                        <Group justify={"center"} mt={10}>
                            <Button style={{backgroundColor:"red"}}>
                                Elutasítás
                            </Button>
                            <Button style={{backgroundColor:"green"}}>
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