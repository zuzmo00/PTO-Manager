import {
    Stack,
    TextInput,
    PasswordInput,
    Group,
    Button,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import AuthContainer from "../components/AuthContainer.tsx";
import useAuth from "../hooks/useAuth.tsx";

const Login = () => {
    const { login } = useAuth();

    const form = useForm({
        initialValues: {
            Email: '',
            password: '',
        },
        validate: {
            Email: (val: string) =>
                val.length < 5 ? 'A törzsszám legalább 5 karakter hosszú kell, hogy legyen' : null,
            password: (val: string) =>
                val.length < 6 ? 'A jelszónak legalább 6 karakter hosszúnak kell lennie.' : null,
        },
    });

    const submit = async () => {
        // előző hibák törlése
        form.clearErrors();

        try {
            await login(form.values.Email, form.values.password);
        } catch (err: any) {
            if (err?.response?.status === 400) {
                form.setErrors({
                    Email: "Hibás Email vagy jelszó.",
                    password: "Hibás Email vagy jelszó.",
                });
            } else {
                form.setErrors({
                    Email: "Ismeretlen hiba történt.",
                    password: "Ismeretlen hiba történt.",
                });
            }
        }
    };

    return (
        <AuthContainer>
            <form onSubmit={form.onSubmit(submit)}>
                <Stack>
                    <TextInput
                        required
                        label="Email"
                        placeholder="Pl. xyzfg@gmail.com"
                        key={form.key('Email')}
                        radius="md"
                        {...form.getInputProps('Email')}
                    />

                    <PasswordInput
                        required
                        label="Jelszó"
                        placeholder="Jelszavad"
                        key={form.key('password')}
                        radius="md"
                        {...form.getInputProps('password')}
                    />
                </Stack>

                <Group justify="center" mt="xl">
                    <Button type="submit" radius="xl">
                        Bejelentkezés
                    </Button>
                </Group>
            </form>
        </AuthContainer>
    );
};

export default Login;