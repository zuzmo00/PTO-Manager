import {
    Box, Button,
    Center,
    Container,
    Divider,
    Group,
    LoadingOverlay,
    Modal,
    Paper,
    Table,
    Text,
    Title
} from "@mantine/core";
import {useEffect, useState} from "react";
import { Calendar } from '@mantine/dates';
import dayjs from "dayjs";
import type ReservedDays from "../Interfaces/ReservedDays.ts";
import 'dayjs/locale/hu';
import type PendingRequests from "../Interfaces/PendingRequests.ts";
import api from "../api/api.ts"
import type RemainingDays from "../Interfaces/RemainingDays.ts";
import { notifications } from "@mantine/notifications";
import {useDisclosure} from "@mantine/hooks";
import type RevokeRequestInputDto from "../Interfaces/RevokeRequestInputDto.ts";

dayjs.locale('hu');

function Landing(){

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [foglaltNapok, setFoglaltNapok] = useState<ReservedDays[] | null>(null);
    const [pendingRequests, setPendingRequests] = useState<PendingRequests[]>([]);
    const [modelOpen, {open, close}] = useDisclosure(false)

    const [selectedBlock, setSelectedBlock] = useState<string>("");
    //const [selectedRequest, setSelectedRequest] = useState<string >("");


    const [szabStat, setSzabStat] = useState<RemainingDays | null>(null);

    const fetchData = async () => {
        setIsLoading(true);
        try{
            const FoglaltNapok_Response = await api.Request.getReservedDays()
            const FoglaltNapok_Res_Data = FoglaltNapok_Response.data?.data ?? [];

            setFoglaltNapok(FoglaltNapok_Res_Data);

            const SzabStat_Response = await api.Request.getRemainingDays();
            setSzabStat(SzabStat_Response.data?.data ?? null);

            const PendingRequests_response = await api.Request.getPendingRequest();
            setPendingRequests(PendingRequests_response.data?.data ?? [])

        }
        catch(err){
            console.log(err);
            setError("Hiba történt a lekérés során.");
            notifications.show({
                title:"Hiba",
                message:"Hiba az adatok lekérése során.",
                color:"red"
            })
        }
        finally {
            setIsLoading(false);
        }
    }


    const HandleRevoke = async(requestId:string) => {
        setIsLoading(true);
        try{
            const inputDto: RevokeRequestInputDto = {RequestBlockId: requestId}
            await api.Request.postRevokeARequest(inputDto)
            await fetchData();
            close();
        }catch (error){
            console.log(error)

            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérdezés során",
                color:"red"
                }
            )
        }finally {
            setIsLoading(false)
        }
    }

    const HandleBlockRevoke = async(requestBlockId:string) => {
        setIsLoading(true);
        try{
            const inputDto: RevokeRequestInputDto = {RequestBlockId: requestBlockId}
            await api.Request.postRevokeRequestBlock(inputDto)
            await fetchData();
            close();
        }catch (error){
            console.log(error)

            notifications.show({
                    title:"Hiba",
                    message:"Hiba történt a lekérdezés során",
                    color:"red"
                }
            )
        }finally {
            setIsLoading(false)
        }
    }


    useEffect(() => {
        fetchData();
    },[])

    return (
        <Container>
            <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
            <Paper p="xl" radius="md" withBorder pos = "relative">
                <Center>
                    <Box style = {{textAlign: "center"}}>
                        <Title mb={20}>
                            Üdvözöljük a Szabadságkezelő Oldalon!
                        </Title>
                        <Text mb={10} style={{fontWeight:"bold"}} >Kérjük válasszon az oldalsávon található menüpontok közül.</Text>
                        {error && <Text c="red">{error}</Text>}
                        {szabStat && (
                            <>
                                <Text>Összes szabadság: {szabStat.allHoliday} nap</Text>
                                <Text>Rendelkezésre álló : {szabStat.remainingDays} nap</Text>
                                <Text>Időarányosan kivető szabadságok száma: {szabStat.timeProportional}</Text>
                            </>
                        )}
                        <Title size="lg" mb="md" mt={10} style={{fontSize:25}}>Szabadság naptár</Title>
                        <Group>
                            <Group gap="xs">
                                <Box
                                    w = {15}
                                    h = {15}
                                    style = {{
                                        backgroundColor: "orangered",
                                        borderRadius: 4,
                                        border: '1px solid rgba(0, 0, 0, 0.1)',
                                    }}
                                />
                                <Text>Különleges munkanap</Text>
                            </Group>
                            <Group gap="xs">
                                <Box
                                w = {15}
                                h = {15}
                                style = {{
                                    backgroundColor: "lightcoral",
                                    borderRadius: 4,
                                    border: '1px solid rgba(0,0,0,0.1)',
                                }}/>
                                <Text>Különleges munkaszüneti nap</Text>
                            </Group>
                            <Group gap="xs">
                                <Box
                                w = {15}
                                h = {15}
                                style = {{
                                    backgroundColor: "forestgreen",
                                    borderRadius: 4,
                                    border: '1px solid rgba(0,0,0,0.1)',
                                }}/>
                                <Text>Szabadság napok</Text>
                            </Group>
                        </Group>
                        <Divider my="lg"/>
                    </Box>
                </Center>
                <Center>
                    <Box style = {{textAlign: "center"}}>
                        <Calendar
                        locale="hu"
                        getDayProps={(date) => {
                            const day = dayjs(date);
                            const daystring = day.format("YYYY-MM-DD");

                            let dayitem: ReservedDays | undefined = undefined


                            const isFoglalt = foglaltNapok?.some(nap => nap.reservedDay === daystring);
                            if(isFoglalt){
                                dayitem = foglaltNapok?.find(cday => cday.reservedDay === daystring);
                            }

                            let backgroundColor: string | undefined;
                            let color : string | undefined;


                            if (!isFoglalt) {
                                //skips this step
                            }else if(dayitem?.reservationType === 5) { //SpecialWorkDay
                                backgroundColor = "orangered"
                                color = "white"
                            }else if(dayitem?.reservationType === 6) { //SpecialHoliday
                                backgroundColor = "lightcoral"
                                color = "white"
                            }else if(isFoglalt) { //SpecialHoliday
                                backgroundColor = "forestgreen"
                                color = "white"
                            }

                            const style: React.CSSProperties = {
                                borderRadius: 4,
                                backgroundColor,
                                color: color
                            };

                            return { style };
                        }}
                        />
                    </Box>
                </Center>
                {pendingRequests.length > 0 && (
                    <Box>
                        <Divider mt={10} my="lg" w="70%" m="auto"/>
                        <Center>
                            <Box>
                                <Title mt={10} style={{fontSize:20, textAlign:"center"}}>Függőben lévő szabadságkérelmek</Title>
                                <Table>
                                    <Table.Thead>
                                        <Table.Tr>
                                            <Table.Th>Sorszám</Table.Th>
                                            <Table.Th>kezdet</Table.Th>
                                            <Table.Th>Vég</Table.Th>
                                            <Table.Th>Kérés időpontja</Table.Th>
                                            <Table.Th></Table.Th>
                                        </Table.Tr>
                                    </Table.Thead>
                                    <Table.Tbody>
                                        {pendingRequests.map(t => (
                                            <Table.Tr key={t.pendingRequestBlock.id}>
                                                <Table.Td>{t.pendingRequestBlock.id}</Table.Td>
                                                <Table.Td>{t.pendingRequestBlock.begin}</Table.Td>
                                                <Table.Td>{t.pendingRequestBlock.end}</Table.Td>
                                                <Table.Td><Button onClick={ () => {setSelectedBlock(t.pendingRequestBlock.id); open();}} style={{backgroundColor:"red"}}>Visszavonás</Button></Table.Td>
                                            </Table.Tr>
                                        ))}
                                    </Table.Tbody>
                                </Table>
                            </Box>
                        </Center>
                    </Box>

                )}
            </Paper>
            <Modal opened={modelOpen} onClose={close} centered title={"Visszavonás"} ta={"center"}>
                <Center>
                    <Box>
                        <Text mb={10} fw={"bold"}>Mely napot szeretné visszavonni az alábbiak közül?</Text>
                        <Divider/>

                        <Table>
                            <Table.Tbody>
                                {(() => {
                                    const found = pendingRequests.find(
                                        (i) => i.pendingRequestBlock.id === selectedBlock
                                    );

                                    if (!found) {
                                        return (
                                            <Table.Tr>
                                                <Table.Td colSpan={2}>Nincs kiválasztott szabadságblokk.</Table.Td>
                                            </Table.Tr>
                                        );
                                    }

                                    return found.requests.map((req) => (
                                        <Table.Tr key={req.id}>
                                            <Table.Td>{req.date}</Table.Td>
                                            <Table.Td><Button style={{backgroundColor:"red"}} onClick={() => HandleRevoke(req.id)}>Visszavon</Button></Table.Td>
                                        </Table.Tr>
                                    ));
                                })()}
                            </Table.Tbody>
                        </Table>
                        <Button mt={10} style={{backgroundColor: "red"}} onClick={ () => HandleBlockRevoke(selectedBlock)}>Összes visszavonása</Button>
                    </Box>
                </Center>
            </Modal>
        </Container>
    );
}

export default Landing;
