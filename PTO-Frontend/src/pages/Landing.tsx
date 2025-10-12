import {
    Box,
    Center,
    Container,
    Divider,
    Group,
    LoadingOverlay,
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

dayjs.locale('hu');

function Landing(){

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [foglaltNapok, setFoglaltNapok] = useState<ReservedDays[] | null>(null);
    const [pendingRequests, setPendingRequests] = useState<PendingRequests[]>([]);

    const [szabStat, setSzabStat] = useState<RemainingDays | null>(null);

    const fetchData = async () => {
        setIsLoading(true);
        try{
            const FoglaltNapok_Response = await api.Request.getReservedDays()
            const FoglaltNapok_Res_Data = FoglaltNapok_Response.data?.data ?? [];
            //sessionStorage.setItem("ReservedDays", JSON.stringify(FoglaltNapok_Res_Data)); Attól függ, hogy akarom megoldani, hogy mindig frissek legyenek az adatok

            setFoglaltNapok(FoglaltNapok_Res_Data);

            const SzabStat_Response = await api.Request.getRemainingDays();
            setSzabStat(SzabStat_Response.data?.data ?? null);

        }
        catch(err){
            console.log(err);
            setError("Hiba történt a lekérés során.");
        }
        finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        fetchData();

        setPendingRequests([]);
    },[])

    return (
        <Container>
            <Paper p="xl" radius="md" withBorder pos = "relative">
                <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
                <Center>
                    <Box style = {{textAlign: "center"}}>
                        <Title mb={20}>
                            Üdvözöljük a Szabadságkezelő Oldalon!
                        </Title>
                        <Text mb={10} style={{fontWeight:"bold"}} >Kérjük válasszon az oldalsávon található menüpontok közül.</Text>
                        {error && <Text c="red">{error}</Text>}
                        {szabStat && (
                            <>
                                <Text>Összes szabadság: {szabStat.osszesSzab} nap</Text>
                                <Text>Rendelkezésre álló (Jelenleg függőben lévő): {szabStat.fuggoben} nap</Text>
                                <Text>Időarányos (jelenleg eddig kivett): {szabStat.eddigKivett}</Text>
                            </>
                        )}
                        <Title size="lg" mb="md" mt={10} style={{fontSize:25}}>Szabadság naptár</Title>
                        <Group>
                            <Group gap="xs">
                                <Box
                                    w = {15}
                                    h = {15}
                                    style = {{
                                        backgroundColor: "blue",
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
                                    backgroundColor: "purple",
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
                            const isHetvegeMunkanap: boolean = true; //Majd ide kell egy api

                            const isFoglalt = foglaltNapok?.some(nap => nap.reservedDay === daystring);
                            if(isFoglalt){
                                dayitem = foglaltNapok?.find(cday => cday.reservedDay === daystring);
                            }

                            let backgroundColor: string | undefined;
                            let color : string | undefined;



                            if(isHetvegeMunkanap && (day.day() === 6 || day.day() === 0)){
                                //backgroundColor = "lightgray"
                                color = "red"
                            }else if (!isFoglalt) {
                                //skips this step
                            }else if(dayitem?.reservationType === 5) { //SpecialWorkDay
                                backgroundColor = "purple"
                                color = "white"
                            }else if(dayitem?.reservationType === 6) { //SpecialHoliday
                                backgroundColor = "blue"
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
                {pendingRequests.length >= 0 && (
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
                                            <Table.Th>Megjegyzés</Table.Th>
                                            <Table.Th>Kérés időpontja</Table.Th>
                                        </Table.Tr>
                                    </Table.Thead>
                                    <Table.Tbody>
                                        <Table.Tr>
                                            <Table.Td>sdf</Table.Td>
                                            <Table.Td>sdf</Table.Td>
                                            <Table.Td>sdf</Table.Td>
                                            <Table.Td>sdf</Table.Td>

                                        </Table.Tr>
                                    </Table.Tbody>
                                </Table>
                            </Box>
                        </Center>
                    </Box>

                )}
            </Paper>
        </Container>
    );
}

export default Landing;
