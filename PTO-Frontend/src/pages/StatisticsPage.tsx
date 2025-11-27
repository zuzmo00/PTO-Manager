import {
    Box, Button,
    Center,
    Container, Divider, Group,
    LoadingOverlay, Modal,
    MultiSelect,
    Paper,
    Text,
    Title
} from "@mantine/core";
import {useEffect, useState} from "react";
import {useDisclosure} from "@mantine/hooks";
import '@mantine/charts/styles.css';
import dayjs from 'dayjs';
import { notifications } from "@mantine/notifications";
import api from "../api/api.ts";

import type DayOutStatisticsDto from "../Interfaces/DayOutStatisticsDto.ts";
import {BarChart, LineChart, PieChart} from '@mantine/charts';
import { DateInput } from '@mantine/dates';


function StatisticsPage () {
    const [isLoading,setIsLoading] = useState(false)
    const [modelOpen, {open, close}] = useDisclosure(false)
    const [pendingData, setPendingData] = useState<DayOutStatisticsDto[] | []> ([])
    const [dropdownValue, setDropdownValue] = useState<string[]|undefined> (undefined)
    const [departmments, setDepartmments] = useState<string[]> ([])
    const [value, setValue] = useState<Date | null>(new Date());

    const templateList:DayOutStatisticsDto[] = [
        {
            department: "IT",
            date: "2025-11-24",
            PTO: 10,
            BetegSzab: 4,
            Kikuldetes: 1
        },
        {
            department: "Pénzügy",
            date: "2025-11-24",
            PTO: 30,
            BetegSzab: 8,
            Kikuldetes: 3
        }
        ,
        {
            department: "HR",
            date: "2025-11-24",
            PTO: 12,
            BetegSzab: 1,
            Kikuldetes: 3
        },

    ];

    const templateListFor2ndChart:DayOutStatisticsDto[] = [
        {
            department: "IT",
            date: "2025-11-24",
            PTO: 10,
            BetegSzab: 4,
            Kikuldetes: 6
        },
        {
            department: "Pénzügy",
            date: "2025-11-25",
            PTO: 30,
            BetegSzab: 0,
            Kikuldetes: 1
        }
        ,
        {
            department: "HR",
            date: "2025-11-26",
            PTO: 16,
            BetegSzab: 6,
            Kikuldetes: 1
        }
    ];

    const templateListFor3ndChart = [
        {
            name: "IT",
            value: 42,
            color:"red"
        },
        {
            name: "HR",
            value: 28,
            color:"green"
        },
        {
            name: "Pénzügy",
            value: 13,
            color:"purple"
        },
    ];
    const fetchData = async () => {
        setIsLoading(true);
        try{
            const departmentdata = await api.Department.getDepartments();
            const l_departments = departmentdata.data?.data ?? []
            setDepartmments(l_departments);

            const requestsByDepartment_data = templateList;
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


    const dataforchart =  () => {
        if (!serachParameters || serachParameters.length === 0) {
            return templateList
        }
       return templateList.filter(c => serachParameters?.includes(c.department));
    }




    useEffect(() => {
        fetchData();

    }, []);


    const [serachParameters, setSearchParameters] = useState<string[] | undefined> (undefined)


    {
        useEffect(() => {
            setSearchParameters(dropdownValue)
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
                    <DateInput
                        label="Dátum"
                        placeholder="Válassz dátumot"
                        value={value}
                        onChange={setValue}
                    />
                </Group>
                <Center>
                    <Box style={{ overflow: "auto"}} mt={20} w="100%">
                        <Divider my="lg"/>
                        <Center mb>
                            <Text fw={"bold"} mb={30}>Szabadságok lebontása részlegenként</Text>
                        </Center>
                        <BarChart
                            h={300}
                            data={dataforchart()}
                            dataKey="department"
                            valueFormatter={(value) => new Intl.NumberFormat('en-US').format(value)}
                            withBarValueLabel
                            series={[
                                { name: 'BetegSzab', color: 'violet.6' },
                                { name: 'PTO', color: 'blue.6' },
                                { name: 'Kikuldetes', color: 'teal.6' },
                            ]}
                            withYAxis={false}
                        />
                    </Box>
                </Center>
                <Divider my="lg"/>
                <Center mb>
                    <Text fw={"bold"} mb={30}>Céges szintű szabadság statisztika</Text>
                </Center>
                    <Box>
                        <LineChart
                            h={300}
                            data={templateListFor2ndChart}
                            dataKey="date"
                            withLegend
                            legendProps={{ verticalAlign: 'bottom', height: 50 }}
                            series={[
                                { name: 'BetegSzab', color: 'violet.6' },
                                { name: 'PTO', color: 'blue.6' },
                                { name: 'Kikuldetes', color: 'teal.6' },
                            ]}
                            curveType="linear"
                        />
                    </Box>
                <Divider my="lg"/>
                <Center>
                    <Text fw={"bold"} mb={10}>Részlegek közötti szabadságmegoszlás</Text>
                </Center>
                <Center>
                    <Box>
                        <PieChart
                            withLabelsLine
                            labelsPosition="outside"
                            labelsType="value"
                            withLabels
                            withTooltip={true}
                            tooltipDataSource="segment"
                            size={200}
                            data={templateListFor3ndChart} />
                        <Group justify="center" mt="md">
                            {templateListFor3ndChart.map((item) => (
                                <Group key={item.name} gap="xs" align="center">
                                    <Box h={15} w={15} bg={item.color} />
                                    <Text fw="bold">{item.name}</Text>
                                </Group>
                            ))}
                        </Group>
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

export default StatisticsPage;